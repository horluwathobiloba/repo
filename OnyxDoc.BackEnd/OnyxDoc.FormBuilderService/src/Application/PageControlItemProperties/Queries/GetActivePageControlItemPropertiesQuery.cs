using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries
{
    public class GetActivePageControlItemPropertiesQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemId { get; set; }
        public string UserId { get; set; }
    }

    public class GetActivePageControlItemPropertiesQueryHandler : IRequestHandler<GetActivePageControlItemPropertiesQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetActivePageControlItemPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetActivePageControlItemPropertiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = await _context.PageControlItemProperties.Where(a => a.SubscriberId == request.SubscriberId && a.PageControlItemId == request.PageControlItemId && a.Status == Status.Active).ToListAsync();
                if (list == null || list.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                var result = _mapper.Map<List<PageControlItemPropertyDto>>(list);

                return Result.Success($"{list.Count} record(s) found.", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving page control item properties. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
