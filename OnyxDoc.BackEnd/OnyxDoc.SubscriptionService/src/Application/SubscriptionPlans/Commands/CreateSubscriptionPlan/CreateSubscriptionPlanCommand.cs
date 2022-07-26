using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Commands;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands;
using ReventInject;
using OnyxDoc.SubscriptionService.Application.Payments.Commands;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Commands
{
    public class CreateSubscriptionPlanCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        [DefaultValue(1)]
        public int NumberOfUsers { get; set; }

        [DefaultValue(5)]
        public int NumberOfTemplates { get; set; }

        [DefaultValue(10)]
        public int StorageSize { get; set; }

        [DefaultValue(StorageSizeType.GB)]
        public StorageSizeType StorageSizeType { get; set; }
        public bool AllowMonthlyPricing { get; set; }
        public bool AllowYearlyPricing { get; set; }
        //public string Period { get; set; }

        [DefaultValue(true)]
        public bool AllowFreeTrial { get; set; }
        public int FreeTrialDays { get; set; }
        public bool IsHighlighted { get; set; }

        [DefaultValue(false)]
        public bool AllowDiscount { get; set; }

        [DefaultValue(false)]
        public bool ShowSubscribeButton { get; set; }

        [DefaultValue(false)]
        public bool ShowContactUsButton { get; set; }

        [DefaultValue(0)]
        public decimal Discount { get; set; }
        public List<UpdateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public List<UpdateSubscriptionPlanFeatureRequest> SubscriptionPlanFeatures { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private INotificationService _notificationService;
        private IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IStripeProductService _stripeProductService;
        private readonly IPaystackProductService _paystackProductService;
        private readonly IFlutterwaveService _flutterwaveService;

        public CreateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator,
            IAuthService authService, IConfiguration configuration, INotificationService notificationService,
            IEmailService emailService, IStripeProductService stripeProductService, IFlutterwaveService flutterwaveService, IPaystackProductService paystackProductService)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _authService = authService;
            _configuration = configuration;
            _notificationService = notificationService;
            _emailService = emailService;
            _stripeProductService = stripeProductService;
            _flutterwaveService = flutterwaveService;
            _paystackProductService =  paystackProductService;
        }

        public async Task<Result> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.SubscriberId, request.UserId, request.UserId);

                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var exists = await _context.SubscriptionPlans.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"Subscription plan name already exists!");
                }

                var entity = new SubscriptionPlan
                {
                    Name = request.Name,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.SubscriberName,
                    AllowDiscount = request.AllowDiscount,
                    AllowFreeTrial = request.AllowFreeTrial,
                    FreeTrialDays = request.FreeTrialDays== 0 ? _configuration["DefaultFreeTrialDays"].ToInt(): request.FreeTrialDays,
                    AllowMonthlyPricing = request.AllowMonthlyPricing,
                    AllowYearlyPricing = request.AllowYearlyPricing,
                    NumberOfTemplates = request.NumberOfTemplates,
                    IsHighlighted = request.IsHighlighted,
                    NumberOfUsers = request.NumberOfUsers,
                    // Period = request.Period,
                    ShowContactUsButton = request.ShowContactUsButton,
                    ShowSubscribeButton = request.ShowSubscribeButton,
                    StorageSize = request.StorageSize,
                    StorageSizeType = request.StorageSizeType,
                    StorageSizeTypeDesc = request.StorageSizeType.ToString(),
                    SubscriptionType = request.SubscriptionType,
                    SubscriptionTypeDesc = request.StorageSizeType.ToString(),
                    UserId = request.UserId,
                    CreatedByEmail = user.Entity.Email,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.SubscriptionPlans.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                //TO DO: create the product in Stripe, Paystack, and Flutterwave

                var notification = new Notification
                {
                    Message = "",
                    NotificationStatus = NotificationStatus.Unread,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now
                };

                //TO DO: Call the API for notifications
                //await _context.Notifications.AddAsync(notification);
                //await _notificationService.SendNotification(request.Email, notification.Message);

                if (request.SubscriptionPlanPricings != null && request.SubscriptionPlanPricings.Count > 0)
                {
                    var command = new UpdateSubscriptionPlanPricingsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = entity.Id,
                        SubscriptionPlanPricings = request.SubscriptionPlanPricings,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };

                    var handler = new UpdateSubscriptionPlanPricingsCommandHandler(_context, _mapper, _authService);
                    var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    //  var recipientsResult = _mediator.Send(command).Result;
                    if (subscriptionFeatureResult.Succeeded == false)
                    {
                        throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    }
                    //Create SubscriptionPlan
                    //await CreateFlutterwavePaymentPlan(request, entity, subscriptionFeatureResult, cancellationToken);
                }

                if (request.SubscriptionPlanFeatures != null && request.SubscriptionPlanFeatures.Count > 0)
                {
                    //save the features
                    var features = request.SubscriptionPlanFeatures
                        .OrderBy(a => a.ParentFeatureId)
                        .ThenBy(a => a.FeatureName).ToList();

                    var command = new UpdateSubscriptionPlanFeaturesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = entity.Id,
                        SubscriptionPlanFeatures = features,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
                    };

                    var handler = new UpdateSubscriptionPlanFeaturesCommandHandler(_context, _mapper, _authService);
                    var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    //  var recipientsResult = _mediator.Send(command).Result;
                    if (subscriptionFeatureResult.Succeeded == false)
                    {
                        throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    }
                }

                await _context.CommitTransactionAsync();
                var result = _mapper.Map<SubscriptionPlanDto>(entity);
                return Result.Success("Subscription plan request created successfully!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Subscription plan request creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private async Task CreateFlutterwavePaymentPlan(CreateSubscriptionPlanCommand request, SubscriptionPlan entity, Result subscriptionFeatureResult, CancellationToken cancellationToken)
        {
            var monthlySubscription = entity.SubscriptionPlanPricings.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.NGN.ToString() && a.PricingPlanType == PricingPlanType.Monthly);
            var flutterwaveMonthlyCommand = new CreateFlutterwavePaymentPlanCommand
            {
                SubscriberId = request.SubscriberId,
                SubscriptionPlanId = entity.Id,
                UserId = request.UserId,
                Amount = monthlySubscription.Amount,
                Duration = _configuration["DaysConfiguration:Month"],
                Interval = monthlySubscription.PricingPlanType.ToString(),
                AccessToken = request.AccessToken
            };
            var flutterwavePaymentPlanHandler = new CreateFlutterwavePaymentPlanCommandHandler(_context, _authService, _flutterwaveService);
            var flutterwaveMonthlyResult = await flutterwavePaymentPlanHandler.Handle(flutterwaveMonthlyCommand, cancellationToken);
            if (flutterwaveMonthlyResult.Succeeded == false)
            {
                throw new Exception(flutterwaveMonthlyResult.Error + subscriptionFeatureResult.Message);
            }
            //yearly
            var yearlySubscription = entity.SubscriptionPlanPricings.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.NGN.ToString() && a.PricingPlanType == PricingPlanType.Yearly);
            var flutterwaveYearlyCommand = new CreateFlutterwavePaymentPlanCommand
            {
                SubscriberId = request.SubscriberId,
                SubscriptionPlanId = entity.Id,
                UserId = request.UserId,
                Amount = monthlySubscription.Amount,
                Duration = _configuration["DaysConfiguration:Year"],
                Interval = monthlySubscription.PricingPlanType.ToString(),
                AccessToken = request.AccessToken
            };
            var flutterwaveYearlyResult = await flutterwavePaymentPlanHandler.Handle(flutterwaveYearlyCommand, cancellationToken);
            if (flutterwaveYearlyResult.Succeeded == false)
            {
                throw new Exception(flutterwaveYearlyResult.Error + subscriptionFeatureResult.Message);
            }
        }
    }
}
