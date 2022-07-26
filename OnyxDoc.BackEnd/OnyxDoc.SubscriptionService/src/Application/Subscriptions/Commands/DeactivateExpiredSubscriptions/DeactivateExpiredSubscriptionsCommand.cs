using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class DeactivateExpiredSubscriptionsCommand : AuthToken, IRequest<Result>
    {
        public int SuperAdminSubscriberId { get; set; }
        public string UserId { get; set; }
    }

    public class DeactivateExpiredSubscriptionsCommandHandler : IRequestHandler<DeactivateExpiredSubscriptionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ISubscriberService _subscriberService;

        public DeactivateExpiredSubscriptionsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, ISubscriberService subscriberService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _subscriberService = subscriberService;
        }

        public async Task<Result> Handle(DeactivateExpiredSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscribers = (await _authService.GetSubscribersAsync(request.AccessToken, request.UserId))?.Entity;

                #region alternative code - DO NOT DELETE!
                //var subscriptions = (await _context.Subscriptions
                //    .Where(x => x.EndDate <= DateTime.Now
                //    && (x.SubscriptionStatus == SubscriptionStatus.Active || x.SubscriptionStatus == SubscriptionStatus.FreeTrial || x.Status == Status.Active))
                //    .ToListAsync()).Select(y => new UpdateSubscriptionStatusesRequest
                //    {
                //        Id = y.Id,
                //        SubscriberId = y.SubscriberId,
                //        SubscriptionPlanId = y.SubscriptionPlanId,
                //        SubscriptionStatus = SubscriptionStatus.Expired
                //    }).ToList();
                #endregion
                var subscriptions = await _context.Subscriptions
                   .Where(x => x.EndDate <= DateTime.Now
                   && (x.SubscriptionStatus == SubscriptionStatus.Active || x.SubscriptionStatus == SubscriptionStatus.FreeTrial || x.Status == Status.Active))
                   .ToListAsync();

                var expiredSubscriptions = (from x in subscriptions
                                            join y in subscribers
                                            on x.SubscriberId equals y.Id
                                            select new UpdateSubscriptionStatusesRequest
                                            {
                                                Id = x.Id,
                                                SubscriberId = x.SubscriberId,
                                                SubscriptionPlanId = x.SubscriptionPlanId,
                                                SubscriptionStatus = SubscriptionStatus.Expired,
                                                Subscriber = y
                                            }).ToList();

                var command = new UpdateSubscriptionStatusesCommand()
                {
                    AccessToken = request.AccessToken,
                    Subscriptions = expiredSubscriptions,
                    SuperAdminSubscriberId = request.SuperAdminSubscriberId,
                    Subscribers = subscribers,
                    UserId = request.UserId
                };
                var result = await new UpdateSubscriptionStatusesCommandHandler(_context, _mapper, _authService, _subscriberService).Handle(command, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Deactivate expired subscriptions failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
