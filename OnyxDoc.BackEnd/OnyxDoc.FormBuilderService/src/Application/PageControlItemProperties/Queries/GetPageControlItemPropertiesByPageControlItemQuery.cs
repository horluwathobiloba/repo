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
    public class GetPageControlItemPropertiesByPageControlItemQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PageControlItemId { get; set; }
        public string UserId { get; set; }
    }
    public class GetPageControlItemPropertiesByFeatureIdQueryHandler : IRequestHandler<GetPageControlItemPropertiesByPageControlItemQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetPageControlItemPropertiesByFeatureIdQueryHandler(IApplicationDbContext context, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetPageControlItemPropertiesByPageControlItemQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = await _context.PageControlItemProperties.Include(a=>a.ControlProperty).Include(a=>a.PageControlItem)
                    .Where(x => x.SubscriberId == request.SubscriberId && x.PageControlItemId == request.PageControlItemId)
                    .ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                var result = _mapper.Map<List<PageControlItemPropertyDto>>(list);
                return Result.Success($"{list.Count} record(s) found.", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get Page control item properties failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
