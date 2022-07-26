using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Application.Utilities.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class ChangeSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SystemSettingId { get; set; }
        public int? ActiveSubscriptionId { get; set; }
        public int NewSubscriptionPlanId { get; set; }
        public string SubscriberName { get; set; }
        public int NewSubscriptionPlanPricingId { get; set; }
        public int? InitialSubscriptionPlanId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int CurrencyId { get; set; }
        public int? PaymentChannelId { get; set; }
        public string CurrencyCode { get; set; }
        public SubscriptionFrequency SubscriptionFrequency { get; set; }
        public int PaymentPeriod { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeSubscriptionCommandHandler : IRequestHandler<ChangeSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ISubscriberService _subscriberService;
        private readonly IConfiguration _configuration;

        public ChangeSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, ISubscriberService subscriberService,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _subscriberService = subscriberService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(ChangeSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currency = new GetCurrencyBySystemSettingsQuery
                {
                    SubscriberId = request.SubscriberId,
                    UserId = request.UserId,
                    SystemSettingsId = request.SystemSettingId
                };

                var subscriptionPlan = await _context.SubscriptionPlans.Where(a=>a.Id == request.NewSubscriptionPlanId && a.Status == Status.Active).FirstOrDefaultAsync();
                if (subscriptionPlan == null)
                {
                    return Result.Failure("Invalid Subscription Plan for Change"); 
                }
                var subscriptionComand = new CreateSubscriptionCommand
                {
                    NumberOfUsers = request.NumberOfUsers,
                    UserId = request.UserId,
                    Amount = request.Amount,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyId = request.CurrencyId,
                    InitialSubscriptionPlanId = request.InitialSubscriptionPlanId,
                    PaymentChannelId = request.PaymentChannelId == 0 ? null : request.PaymentChannelId,
                    AccessToken = request.AccessToken,
                    PaymentPeriod = request.PaymentPeriod,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = request.SubscriberName,
                    SubscriptionType = request.SubscriptionType,
                    SubscriptionFrequency = request.SubscriptionFrequency,
                    SubscriptionPlanId = request.NewSubscriptionPlanId,
                    SubscriptionPlanPricingId = request.NewSubscriptionPlanPricingId,
                    IsFreeTrial = false,
                    ActiveSubscriptionId = request.ActiveSubscriptionId,
                    IsASubscriptionChange = true
                };
                var result = await new CreateSubscriptionCommandHandler(_context, _mapper, _authService, _configuration, _subscriberService)
                    .Handle(subscriptionComand, cancellationToken);
                if (!result.Succeeded)
                {
                    return Result.Failure(result.Message);
                }
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription Plan Change failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }
}
