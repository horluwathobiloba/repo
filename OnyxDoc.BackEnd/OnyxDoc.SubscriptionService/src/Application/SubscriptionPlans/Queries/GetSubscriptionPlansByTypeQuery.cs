using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries
{
    public class GetSubscriptionPlansByTypeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; } 
        public SubscriptionType SubscriptionType { get; set; }
        public string UserId { get; set; }
    }
    public class GetSubscriptionPlansByTypeQueryHandler : IRequestHandler<GetSubscriptionPlansByTypeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetSubscriptionPlansByTypeQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetSubscriptionPlansByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.SubscriptionPlans.Include(a => a.SubscriptionPlanPricings).Include(a => a.SubscriptionPlanFeatures)
                    .Where(x => x.SubscriberId == request.SubscriberId 
                    && x.SubscriptionType == request.SubscriptionType).ToListAsync();

                if (entity == null || entity.Count == 0)
                {
                    return Result.Failure("No SubscriptionPlans available");
                }
                return Result.Success($"{entity.Count}(s) found.", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get SubscriptionPlans failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
