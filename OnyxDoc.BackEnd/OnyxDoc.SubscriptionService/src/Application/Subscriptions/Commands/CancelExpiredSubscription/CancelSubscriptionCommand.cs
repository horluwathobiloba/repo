using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Application.Utilities.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class CancelSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SystemSettingId { get; set; }
    }

    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ISubscriberService _subscriberService;
        private readonly IConfiguration _configuration;
        private readonly IPaystackSubscriptionService _paystackSubscriptionService;
        private readonly IStripeSubscriptionService _stripeSubscriptionService;

        public CancelSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, ISubscriberService subscriberService,
            IConfiguration configuration, IPaystackSubscriptionService paystackSubscriptionService, IStripeSubscriptionService stripeSubscriptionService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _subscriberService = subscriberService;
            _configuration = configuration;
            _paystackSubscriptionService = paystackSubscriptionService;
            _stripeSubscriptionService = stripeSubscriptionService;
        }

        public async Task<Result> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currency = new GetCurrencyBySystemSettingsQuery
                {
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    SystemSettingsId = request.SystemSettingId
                };

                //get the subscription first to determine the payment channel id
                var subscription = await _context.Subscriptions.Include(a => a.Currency).Include(c => c.PaymentChannel).FirstOrDefaultAsync(x => x.Id == request.Id);
                var pgSubcription = await _context.PGSubscriptions.FirstOrDefaultAsync(x => x.SubscriptionId == subscription.Id);

                Result result = new Result();

                switch (subscription.Currency.CurrencyCode)
                {
                    case CurrencyCode.NGN:
                        if(subscription.PaymentChannel == null)
                        {
                            return Result.Failure($"Payment ChannelId is null");
                        }    
                        switch (subscription.PaymentChannel.PaymentGateway)
                        {
                            case PaymentGateway.Paystack:
                                result = await _paystackSubscriptionService.DisableSubscription(pgSubcription.PaymentGatewaySubscriptionCode,null);
                                break;
                            case PaymentGateway.Flutterwave:
                                //To do for Oluchi
                                break;
                            case PaymentGateway.KoralPay:
                                //To do for Babatola
                                break;
                            case PaymentGateway.Ruby:
                                //No implementation for now
                                break;
                            case PaymentGateway.Bank:
                                //No implementation for now NGN, GHS, ZAR or USD
                                break;
                            default:
                                break;
                        }
                        break;
                    case CurrencyCode.GHS:
                    case CurrencyCode.ZAR:
                        switch (subscription.PaymentChannel.PaymentGateway)
                        {
                            case PaymentGateway.Paystack:
                                result = await _paystackSubscriptionService.DisableSubscription(subscription.PaymentGatewaySubscriptionId, null);
                                break;
                            case PaymentGateway.Flutterwave:
                                //To do for Oluchi
                                break;
                            default:
                                break;
                        }
                        break;
                    case CurrencyCode.USD:
                    case CurrencyCode.GBP:
                    case CurrencyCode.EUR:
                        result = await _stripeSubscriptionService.CancelSubscription(subscription.PaymentGatewaySubscriptionId);
                        break;
                    default:
                        switch (subscription.PaymentChannel.PaymentGateway)
                        {
                            case PaymentGateway.Stripe:
                                result = await _stripeSubscriptionService.CancelSubscription(pgSubcription.PaymentGatewaySubscriptionCode);
                                break;
                            case PaymentGateway.Paystack:
                                result = await _paystackSubscriptionService.DisableSubscription(pgSubcription.PaymentGatewaySubscriptionCode, null);
                                break;

                            case PaymentGateway.Flutterwave:
                                //To do for Oluchi
                                break;
                            default:
                                break;
                        }
                        break;
                }

                if (result.Succeeded)
                {
                    var command = new UpdateSubscriptionStatusCommand()
                    {
                        AccessToken = request.AccessToken,
                        Id = request.Id,
                        SubscriptionStatus = SubscriptionStatus.Cancelled,
                        SubscriberId = request.SubscriberId,
                        UserId = request.UserId
                    };
                    Result updateResult = await new UpdateSubscriptionStatusCommandHandler(_context, _mapper, _authService, _configuration, _subscriberService).Handle(command, cancellationToken);

                    return updateResult;
                }

                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription deactivation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
