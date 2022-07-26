using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries
{
    public class GetPageControlItemPropertyValueByIdQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetPageControlItemPropertyValueByIdQueryHandler : IRequestHandler<GetPageControlItemPropertyValueByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPageControlItemPropertyValueByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPageControlItemPropertyValueByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var PageControlItemPropertyValue = await _context.PageControlItemPropertyValues.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.Id == request.Id);
                if (PageControlItemPropertyValue == null)
                {
                    return Result.Failure("Invalid page control item property value specified");
                }

                var result = _mapper.Map<PageControlItemPropertyValueDto>(PageControlItemPropertyValue);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item property value. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
