using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries
{
    public class GetSubscriptionPlanFeaturesBySubscriptionQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string UserId { get; set; }
    }
    public class GetSubscriptionPlanFeaturesBySubscriptionQueryHandler : IRequestHandler<GetSubscriptionPlanFeaturesBySubscriptionQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public GetSubscriptionPlanFeaturesBySubscriptionQueryHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(GetSubscriptionPlanFeaturesBySubscriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var subscriber = new SubscriberDto();

                subscriber = _authService.Subscriber;
                var subscriptionPlanFeatures = await _context.SubscriptionPlanFeatures.Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId).ToListAsync();

                if (subscriptionPlanFeatures == null || subscriptionPlanFeatures.Count == 0)
                {
                    return Result.Failure("No record found");
                }
                return Result.Success($"{subscriptionPlanFeatures.Count}(s) found.", subscriptionPlanFeatures);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Get subscription plan features failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
