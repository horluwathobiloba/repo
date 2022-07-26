using AutoMapper; 
using MediatR;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models; 
using System;
using System.Threading;
using System.Threading.Tasks;


namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class SignUpAndSubscribeCommand : AuthToken, IRequest<Result>
    {
        public CreateSubscriptionRequest Subscription { get; set; }
        public CreateSubscriberRequest Subscriber { get; set; }
    }

    public class SignUpAndSubscribeCommandHandler : IRequestHandler<SignUpAndSubscribeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ISubscriberService _subscriberService;
        private readonly IConfiguration _configuration; 


        public SignUpAndSubscribeCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration, ISubscriberService subscriberService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _subscriberService = subscriberService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(SignUpAndSubscribeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var signUpRespo = await _subscriberService.SignUpSubscriberAsync(request.AccessToken, request.Subscriber);

                var subscriber = _subscriberService?.SubscriberObject;
                var adminUser = _subscriberService?.SubscriberAdmin;
                if (subscriber != null && subscriber.id > 0)
                {
                    var subscriptionCommand = this.GetSubscriptionCommand(request);
                    subscriptionCommand.SubscriberId = subscriber.id;
                    subscriptionCommand.SubscriberName = subscriber.name;
                    subscriptionCommand.SubscriberObject = subscriber;
                    subscriptionCommand.UserId = adminUser.userId;
                    var result = await new CreateSubscriptionCommandHandler(_context, _mapper, _authService, _configuration, _subscriberService).Handle(subscriptionCommand, cancellationToken);
                    if (!result.Succeeded)
                    {
                        return Result.Failure("Sign Up Failed due to :" + result.Message);
                    }
                    var response = (Subscriber: subscriber, result.Entity);
                    return Result.Success("Sign up new subscriber and subscription was successful.", new { subscriber = response.Subscriber, subscription = response.Entity, user = adminUser });
                }
                else
                {
                    if (signUpRespo.Message != null)
                    {
                        return Result.Failure(signUpRespo.Message);
                    }
                    if (signUpRespo.Messages != null)
                    {
                        return Result.Failure(signUpRespo.Messages[0]);
                    }
                    return Result.Failure(signUpRespo);
                }

            }
            catch (Exception ex)
            {
                //delete subscriber details
                return Result.Failure($"Sign up and subscribe failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        public CreateSubscriptionCommand GetSubscriptionCommand(SignUpAndSubscribeCommand request)
        {
            var command = new CreateSubscriptionCommand
            {
                AccessToken = request.AccessToken,
                Amount = request.Subscription.Amount,
                SubscriptionPlanPricingId = request.Subscription.SubscriptionPlanPricingId,
                CurrencyCode = request.Subscription.CurrencyCode,
                CurrencyId = request.Subscription.CurrencyId,
                InitialSubscriptionPlanId = request.Subscription.InitialSubscriptionPlanId,
                RenewedSubscriptionPlanId = request.Subscription.RenewedSubscriptionPlanId,
                PaymentChannelId = request.Subscription.PaymentChannelId,
                SubscriberId = request.Subscription.SubscriberId,
                SubscriberName = request.Subscription.SubscriberName,
                SubscriptionPlanId = request.Subscription.SubscriptionPlanId,
                SubscriptionType = request.Subscription.SubscriptionType,
                IsFreeTrial = request.Subscription.IsFreeTrial,
                UserId = request.Subscription.UserId,
                PaymentPeriod = request.Subscription.PaymentPeriod
            };
            return command;
        }


    }
}
