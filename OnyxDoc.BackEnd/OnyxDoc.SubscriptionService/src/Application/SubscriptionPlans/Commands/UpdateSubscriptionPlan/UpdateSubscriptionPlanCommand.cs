using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Commands;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReventInject;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands;
using OnyxDoc.SubscriptionService.Application.Payments.Commands;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Commands
{
    public class UpdateSubscriptionPlanCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public SubscriptionType SubscriptionType { get; set; } 
        public int NumberOfUsers { get; set; }
        public int NumberOfTemplates { get; set; }
        public int StorageSize { get; set; }
        public StorageSizeType StorageSizeType { get; set; } 
        public bool AllowMonthlyPricing { get; set; }
        public bool AllowYearlyPricing { get; set; }
        //public string Period { get; set; }
        public bool AllowFreeTrial { get; set; }
        public int FreeTrialPeriod { get; set; }
        public FreeTrialPeriodFrequency FreeTrialPeriodFrequency { get; set; }
        public bool AllowDiscount { get; set; }
        public bool ShowSubscribeButton { get; set; }
        public bool ShowContactUsButton { get; set; }
        public decimal Discount { get; set; }
        public List<UpdateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public List<UpdateSubscriptionPlanFeatureRequest> SubscriptionPlanFeatures { get; set; }
        public string UserId { get; set; }
        public bool IsHighlighted { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IFlutterwaveService _flutterwaveService;

        public UpdateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration, IFlutterwaveService flutterwaveService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _configuration = configuration;
            _flutterwaveService = flutterwaveService;
        }
        public async Task<Result> Handle(UpdateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.SubscriptionPlans.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription plan specified.");
                }

                //var subscriptionExists = await _context.SubscriptionPlans.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id && x.Name == request.Name);

                //if (!subscriptionExists)
                //{
                //    return Result.Failure($"Another subscription plan named {request.Name.ToString()} already exists.");
                //}

                entity.Name = request.Name.ToString();
                entity.AllowDiscount = request.AllowDiscount;
                entity.AllowFreeTrial = request.AllowFreeTrial;
                entity.AllowMonthlyPricing = request.AllowMonthlyPricing;
                entity.AllowYearlyPricing = request.AllowYearlyPricing;
                entity.NumberOfTemplates = request.NumberOfTemplates;
                entity.NumberOfUsers = request.NumberOfUsers;
                entity.FreeTrialDays = entity.FreeTrialDays == 0 ? _configuration["DefaultFreeTrialDays"].ToInt() : entity.FreeTrialDays;
                entity.ShowContactUsButton = request.ShowContactUsButton;
                entity.ShowSubscribeButton = request.ShowSubscribeButton;
                entity.StorageSize = request.StorageSize;
                entity.StorageSizeType = request.StorageSizeType;
                entity.StorageSizeTypeDesc = request.StorageSizeType.ToString();
                entity.SubscriptionType = request.SubscriptionType;
                entity.SubscriptionTypeDesc = request.StorageSizeType.ToString();
                entity.Name = request.Name;
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.IsHighlighted = request.IsHighlighted;

                await _context.BeginTransactionAsync();

                _context.SubscriptionPlans.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

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
               //update flutterwave plan name 
                if (request.SubscriptionPlanPricings != null && request.SubscriptionPlanPricings.Count > 0)
                {
                    //save the pricings
                    var pricings = request.SubscriptionPlanPricings
                        .OrderBy(a => a.CurrencyCode)
                        .ThenBy(a => a.IsDeleted).ToList();

                    var command = new UpdateSubscriptionPlanPricingsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.Id,
                        SubscriptionPlanPricings = request.SubscriptionPlanPricings,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken,

                    };

                    var handler = new UpdateSubscriptionPlanPricingsCommandHandler(_context, _mapper, _authService);
                    var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    //  var recipientsResult = _mediator.Send(command).Result;
                    if (subscriptionFeatureResult.Succeeded == false)
                    {
                        throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    }
                    //update plan on flutterwaves end
                    //Create SubscriptionPlan
                    //get flutterwave plans and delete
                    var paymentPlans = await _context.FlutterwavePaymentPlans.Where(a=>a.SubscriptionPlanId == request.Id).ToListAsync();
                    if (paymentPlans != null || paymentPlans.Count() > 0)
                    {
                        await DeleteAndCreatePaymentPlan(request, entity, subscriptionFeatureResult, cancellationToken, request.UserId);
                    }
                    //create new plans
                    //Create SubscriptionPlan
                    await CreateFlutterwavePaymentPlan(request, entity, subscriptionFeatureResult, cancellationToken);
                }

                if (request.SubscriptionPlanFeatures != null && request.SubscriptionPlanFeatures.Count > 0)
                {
                    //save the features
                    var features = request.SubscriptionPlanFeatures
                        .OrderBy(a => a.ParentFeatureId)
                        .ThenBy(a => a.ParentFeatureId).ToList();

                    var command = new UpdateSubscriptionPlanFeaturesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.Id,
                        SubscriptionPlanFeatures = request.SubscriptionPlanFeatures,
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
                return Result.Success("Subscription plan update was successful!", result);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Subscription plan update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private async Task DeleteAndCreatePaymentPlan(UpdateSubscriptionPlanCommand request, SubscriptionPlan entity, Result subscriptionFeatureResult,
            CancellationToken cancellationToken, string userId)
        {
            //monthly
            var monthlySubscription = entity.SubscriptionPlanPricings.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.NGN.ToString() && a.PricingPlanType == PricingPlanType.Monthly);
            var flutterwaveMonthlyCommand = new CancelFlutterwavePaymentPlanCommand
            {
                SubscriberId = request.SubscriberId,
                SubscriptionPlanId = entity.Id,
                CurrencyCode = CurrencyCode.NGN.ToString(),
                UserId = request.UserId,
                AccessToken = request.AccessToken
            };
            var cancelFlutterwaveMonthlyPaymentPlanHandler = new CancelFlutterwavePaymentPlanCommandHandler(_context, _authService, _flutterwaveService);
            var flutterwaveMonthlyResult = await cancelFlutterwaveMonthlyPaymentPlanHandler.Handle(flutterwaveMonthlyCommand, cancellationToken);
            if (flutterwaveMonthlyResult.Succeeded == false)
            {
                throw new Exception(flutterwaveMonthlyResult.Error + subscriptionFeatureResult.Message);
            }
            //yearly
            var yearlySubscription = entity.SubscriptionPlanPricings.FirstOrDefault(a => a.CurrencyCode == CurrencyCode.NGN.ToString() && a.PricingPlanType == PricingPlanType.Yearly);
            var flutterwaveYearlyCommand = new CancelFlutterwavePaymentPlanCommand
            {
                SubscriberId = request.SubscriberId,
                SubscriptionPlanId = entity.Id,
                CurrencyCode = CurrencyCode.NGN.ToString(),
                UserId = request.UserId,
                AccessToken = request.AccessToken
            };
            var cancelFlutterwaveYearlyPaymentPlanHandler = new CancelFlutterwavePaymentPlanCommandHandler(_context, _authService, _flutterwaveService);
            var flutterwaveYearlyResult = await cancelFlutterwaveYearlyPaymentPlanHandler.Handle(flutterwaveMonthlyCommand, cancellationToken);
            if (flutterwaveYearlyResult.Succeeded == false)
            {
                throw new Exception(flutterwaveYearlyResult.Error + subscriptionFeatureResult.Message);
            }
        }

        private async Task CreateFlutterwavePaymentPlan(UpdateSubscriptionPlanCommand request, SubscriptionPlan entity, Result subscriptionFeatureResult, CancellationToken cancellationToken)
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

