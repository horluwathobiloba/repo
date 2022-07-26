using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Subscriptions.Queries;
using OnyxDoc.SubscriptionService.Application.Utilities.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using ReventInject;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class CreateSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SystemSettingId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public int? InitialSubscriptionPlanId { get; set; }
        public int? RenewedSubscriptionPlanId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int CurrencyId { get; set; }
        public int? PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public SubscriptionFrequency SubscriptionFrequency { get; set; }
        public int PaymentPeriod { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal Amount { get; set; }

        public bool IsFreeTrial { get; set; }

        [JsonIgnore]
        public SubscriberObjectDto SubscriberObject { get; set; }

        [JsonIgnore]
        public int? ActiveSubscriptionId { get; set; }

        [JsonIgnore]
        public bool IsASubscriptionChange { get; set; }

        [JsonIgnore]
        public bool IsSubscriptionRenewal { get; set; }
        [JsonIgnore]
        public DateTime ActiveSubscriptionEndDate { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ISubscriberService _subscriberService;

        public CreateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, IConfiguration configuration, ISubscriberService subscriberService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _subscriberService = subscriberService;
        }

        public async Task<Result> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = new SubscriberDto();


                var currency = new GetCurrencyBySystemSettingsQuery
                {
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    SystemSettingsId = request.SystemSettingId
                };

                if (request.SubscriberObject != null)
                {
                    subscriber.Id = request.SubscriberObject.id;
                    subscriber.HasActivatedFreeTrial = request.SubscriberObject.hasActivatedFreeTrial;
                    subscriber.FreeTrialCompleted = request.SubscriberObject.freeTrialCompleted;
                }
                else
                {
                    var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                    subscriber = _authService.Subscriber;
                }

                ///first check if the subscriber has activated free trial before now and if the free trial is completed or not. 
                if (subscriber.HasActivatedFreeTrial && !subscriber.FreeTrialCompleted)
                {
                    return Result.Failure($"Your subscriber profile has activated a frial trial before now. Kindly pay to upgrade your subscription plan and continue enjoying your product.");
                }
                else if (subscriber.FreeTrialCompleted)
                {
                    return Result.Failure($"Your frial trial period has ended before now. Kindly pay to upgrade and continue enjoying your product.");
                }

                SubscriptionPlan subscriptionPlan = await _context.SubscriptionPlans.Include(a=>a.SubscriptionPlanPricings).FirstOrDefaultAsync(a => a.Id == request.SubscriptionPlanId && a.Status == Status.Active);

                if (subscriptionPlan == null || subscriptionPlan.Id <= 0)
                {
                    return Result.Failure($"Subscription plan does not exist.");
                }
                if (request.IsFreeTrial && !subscriptionPlan.AllowFreeTrial)
                {
                    return Result.Failure($"This subscription plan does not allow free trial.");
                }
                var subscriptionPlanPricing = subscriptionPlan.SubscriptionPlanPricings.FirstOrDefault(a => a.CurrencyCode == request.CurrencyCode);
                if (subscriptionPlanPricing == null)
                {
                    return Result.Failure($"This subscription plan does not have a valid subscription plan pricing.");
                }
                await _context.BeginTransactionAsync();

                Subscription entity = new Subscription
                {
                    PaymentStatusDesc = PaymentStatus.Initiated.ToString(),
                    PaymentStatus = PaymentStatus.Initiated,
                    CreatedByEmail = subscriber.ContactEmail, //Changed  subscriber.Email to subscriber.ContactEmail
                    SubscriptionNo = Xtenxion.GenerateUniqueID().ToUpper(),
                    Name = request.SubscriberId + "_" + request.SubscriptionPlanId,
                    SubscriberId = request.SubscriberId,
                    SubscriptionPlanPricingId = subscriptionPlanPricing.Id,
                    SubscriberName = subscriber?.Name,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyId = request.CurrencyId,
                    InitialSubscriptionPlanId = request.InitialSubscriptionPlanId,
                    PaymentChannelId = request.PaymentChannelId == 0 ? null : request.PaymentChannelId,
                    RenewedSubscriptionPlanId = request.RenewedSubscriptionPlanId,
                    SubscriptionType = request.SubscriptionType,
                    SubscriptionTypeDesc = request.SubscriptionType.ToString(),
                    SubscriptionFrequency = request.SubscriptionFrequency,
                    SubscriptionFrequencyDesc = request.SubscriptionFrequency.ToString(),
                    PaymentPeriod = request.PaymentPeriod,
                    NumberOfUsers = request.NumberOfUsers,
                    Amount = request.Amount,
                    FreeTrialActivated = request.IsFreeTrial,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now
                };

                //check if the subscription plan allows freetrial
                if (request.IsFreeTrial && subscriptionPlan.AllowFreeTrial)
                {
                    entity.TrialEndDate = DateTime.Now.AddDays(subscriptionPlan.FreeTrialDays);
                    entity.SubscriptionStatus = SubscriptionStatus.FreeTrial;
                    entity.StartDate = DateTime.Now;
                    int period = subscriptionPlan.FreeTrialDays == 0 ? _configuration["DefaultFreeTrialDays"].ToInt() : subscriptionPlan.FreeTrialDays;
                    entity.EndDate = DateTime.Now.AddDays(period);
                    entity.Status = Status.Active;
                    entity.StatusDesc = entity.Status.ToString();
                }
                else if (!request.IsFreeTrial)
                {
                    entity.SubscriptionStatus = SubscriptionStatus.ProcessingPayment;
                    entity.SubscriptionStatusDesc = SubscriptionStatus.ProcessingPayment.ToString();
                    entity.StartDate = DateTime.Now;
                    entity.EndDate = CalculateEndDate(entity, subscriptionPlan);
                    entity.Status = Status.Inactive;
                    /*entity.Status = Status.Active;*/

                    entity.StatusDesc = entity.Status.ToString();
                }
                if (request.IsASubscriptionChange || request.IsSubscriptionRenewal)
                {
                    var activeSubscription = new Subscription();
                    if (request.ActiveSubscriptionId != null)
                    {
                        activeSubscription = await _context.Subscriptions.Where(a => a.Id == request.ActiveSubscriptionId &&
                                                       a.SubscriberId == request.SubscriberId && a.Status == Status.Active).FirstOrDefaultAsync();
                        if (activeSubscription == null)
                        {
                            return Result.Failure("Invalid Active Subscription");
                        }
                        entity.SubscriptionStatus = SubscriptionStatus.PendingActivation;
                        entity.SubscriptionStatusDesc = SubscriptionStatus.PendingActivation.ToString();
                        entity.StartDate = activeSubscription.EndDate.AddDays(1);
                        entity.EndDate = CalculateEndDate(entity, subscriptionPlan);
                        entity.Status = Status.Inactive;
                        entity.StatusDesc = entity.Status.ToString();
                        if (request.IsSubscriptionRenewal)
                        {
                            entity.RenewedSubscriptionPlanId = activeSubscription.SubscriptionPlanId;
                        }
                    }
                    var previousSubscription = new Subscription();
                    previousSubscription = await _context.Subscriptions.Where(a => a.SubscriberId == request.SubscriberId).OrderByDescending(a=>a.CreatedDate).FirstOrDefaultAsync();
                    if (previousSubscription != null)
                    {
                        entity.RenewedSubscriptionPlanId = previousSubscription.SubscriptionPlanId;
                    }
                }
                
                if (entity.SubscriptionType == SubscriptionType.Individual)
                {
                    entity.NumberOfUsers = 1;
                    entity.TotalAmount = request.Amount * entity.NumberOfUsers.Value;
                }
                else if (entity.SubscriptionType == SubscriptionType.Corporate)
                {
                    entity.NumberOfUsers = request.NumberOfUsers;
                    entity.TotalAmount = request.Amount * entity.NumberOfUsers.Value;
                }

                //TransactionReference = request.TransactionReference,
                //PaymentChannelReference = request.PaymentChannelReference,
                //PaymentChannelResponse = request.PaymentChannelResponse,
                //PaymentChannelStatus = request.PaymentChannelStatus,
                //PaymentStatus = request.PaymentStatus,
                //PaymentStatusDesc = request.PaymentStatus.ToString(),

                bool exists = await _context.Subscriptions.AnyAsync(a => a.SubscriberId == request.SubscriberId
              && a.SubscriptionPlanId == request.SubscriptionPlanId && a.SubscriptionNo == entity.SubscriptionNo);

                if (exists)
                {
                    return Result.Failure($"Subscription with number:'{entity.SubscriptionNo}' already exists.");
                }

                //execute an update request to the subscriber profile to update the free trial activated field
                //var freeTrialUpdateResult = await _subscriberService.ActivateSubscriberFreeTrialAsync(request.AccessToken, request.SubscriberId, request.UserId);

                //if (!freeTrialUpdateResult.Succeeded || freeTrialUpdateResult.Entity == null)
                //{
                //    return Result.Failure(freeTrialUpdateResult.Message);
                //}

                await _context.Subscriptions.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<SubscriptionDto>(entity);
                return Result.Success("Subscription created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription creation failed. Error: { ex?.Message + " "+ ex?.InnerException?.Message}");
            }
        }
        private DateTime CalculateEndDate(Subscription subscription, SubscriptionPlan subscriptionPlan)
        {
            subscription.PaymentPeriod = subscription.PaymentPeriod <= 0 ? 1 : subscription.PaymentPeriod;

            switch (subscription.SubscriptionFrequency)
            {
                case SubscriptionFrequency.Monthly:
                    if (subscriptionPlan.AllowMonthlyPricing)
                    {
                        return subscription.StartDate.AddMonths(subscription.PaymentPeriod);
                    }
                    else
                    {
                        throw new Exception($"Subscription plan '{subscriptionPlan.Name}' does not allow for monthly billing.");
                    }
                case SubscriptionFrequency.Yearly:

                    if (subscriptionPlan.AllowYearlyPricing)
                    {
                        return subscription.StartDate.AddYears(subscription.PaymentPeriod);
                    }
                    else
                    {
                        throw new Exception($"Subscription plan '{subscriptionPlan.Name}' does not allow for yearly billing.");
                    }
                default:
                    return subscription.StartDate.AddYears(subscription.PaymentPeriod);
            }
        }
    }


}
