using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Subscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class RenewSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }

      
    }

    public class RenewSubscriptionCommandHandler : IRequestHandler<RenewSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ISubscriberService _subscriberService;
        public RenewSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration, ISubscriberService subscriberService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _subscriberService = subscriberService;
        }

        public async Task<Result> Handle(RenewSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                //get last subscription of user and renew
                var lastSubscription = await _context.Subscriptions.Include(a=>a.SubscriptionPlan).
                    Where(a=>a.SubscriberId == request.SubscriberId).OrderByDescending(a=>a.CreatedDate).FirstOrDefaultAsync();
                if (lastSubscription == null)
                {
                    return Result.Failure("There is no previous subscription for renewal");
                }

                var subscriptionComand = new CreateSubscriptionCommand
                {
                    NumberOfUsers = lastSubscription.NumberOfUsers.Value,
                    UserId = lastSubscription.UserId,
                    Amount = lastSubscription.Amount,
                    CurrencyCode = lastSubscription.CurrencyCode,
                    CurrencyId = lastSubscription.CurrencyId,
                    InitialSubscriptionPlanId = lastSubscription.InitialSubscriptionPlanId,
                    PaymentChannelId = lastSubscription.PaymentChannelId,
                    AccessToken = request.AccessToken,
                    PaymentPeriod = lastSubscription.PaymentPeriod,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = lastSubscription.SubscriberName,
                    SubscriptionType = lastSubscription.SubscriptionType,
                    SubscriptionFrequency = lastSubscription.SubscriptionFrequency,
                    SubscriptionPlanId = lastSubscription.SubscriptionPlanId,
                    SubscriptionPlanPricingId = lastSubscription.SubscriptionPlanPricingId,
                    IsSubscriptionRenewal = true
                };
                var result = await new CreateSubscriptionCommandHandler(_context, _mapper, _authService, _configuration, _subscriberService)
                    .Handle(subscriptionComand, cancellationToken);
                if (!result.Succeeded)
                {
                    return Result.Failure(result.Message);
                }

                return Result.Success("Subscription renewed successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription renewal failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
