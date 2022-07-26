using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReventInject;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries
{
    public class GetPageControlItemPropertyValuesDynamicQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SearchText { get; set; }
        public string UserId { get; set; }
    }

    public class GetPageControlItemPropertyValuesDynamicQueryHandler : IRequestHandler<GetPageControlItemPropertyValuesDynamicQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPageControlItemPropertyValuesDynamicQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPageControlItemPropertyValuesDynamicQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId,  request.UserId);
                var list = await _context.PageControlItemPropertyValues.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    list = list.Where(a => request.SearchText.IsIn(a.Value.ToString()) 
                    || request.SearchText.IsIn(a.Name)).ToList();
                }

                if (list == null)
                {
                    throw new NotFoundException(nameof(PageControlItemPropertyValue));
                }
                var result = _mapper.Map<List<PageControlItemPropertyValueDto>>(list);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item property values. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }

}
