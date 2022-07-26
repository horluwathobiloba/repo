using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class CreateSubscriptionPlanPricingCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public PricingPlanType PricingPlanType { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }

        public string UserId { get; set; }
    }

    public class CreateSubscriptionCurrencyCommandHandler : IRequestHandler<CreateSubscriptionPlanPricingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateSubscriptionPlanPricingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.SubscriptionPlanPricings.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && (a.CurrencyId == request.CurrencyId || a.CurrencyCode == request.CurrencyCode));

                if (exists)
                {
                    return Result.Failure($"Subscription pricing '{request.CurrencyCode}' already exists.");
                }

                SubscriptionPlan subscriptionPlan = await _context.SubscriptionPlans.FirstOrDefaultAsync(a => a.Id == request.SubscriptionPlanId);

                var entity = new SubscriptionPlanPricing
                {
                    Name = request.SubscriptionPlanId + "_" + request.CurrencyCode,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyId = request.CurrencyId,                    
                    PricingPlanType = request.PricingPlanType,
                    PricingPlanTypeDesc = request.PricingPlanType.ToString(),
                    Amount = request.Amount,
                    Discount = subscriptionPlan.AllowDiscount ? request.Discount : 0,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.SubscriptionPlanPricings.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionPlanPricingDto>(entity);
                return Result.Success("Subscription pricing created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription plan pricing creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
