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

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Queries
{
    public class GetPageControlItemQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int DocumentPageId { get; set; } 
        public int ControlId { get; set; }
        public string UserId { get; set; }
    }

    public class GetPageControlItemQueryHandler : IRequestHandler<GetPageControlItemQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPageControlItemQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPageControlItemQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PageControlItems.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId 
                && a.DocumentPageId ==  request.DocumentPageId &&  a.ControlId == request.ControlId);

                if (entity == null)
                {
                    return Result.Failure("No record found!");
                }

                var result = _mapper.Map<PageControlItemDto>(entity);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
