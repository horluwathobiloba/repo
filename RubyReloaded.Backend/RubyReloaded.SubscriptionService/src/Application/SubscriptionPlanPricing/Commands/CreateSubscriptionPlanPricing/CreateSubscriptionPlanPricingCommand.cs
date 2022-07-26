using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class CreateSubscriptionPlanPricingCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }

        public bool EnableMonthlyPricingPlan { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal MonthlyDiscount { get; set; }
        public bool EnableYearlyPricingPlan { get; set; }
        public decimal YearlyAmount { get; set; }
        public decimal YearlyDiscount { get; set; } 

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
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var exists = await _context.SubscriptionPlanPricings.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && (a.CurrencyId == request.CurrencyId || a.CurrencyCode == request.CurrencyCode));

                if (exists)
                {
                    return Result.Failure($"Subscription pricing '{request.CurrencyCode}' already exists.");
                }

                var entity = new SubscriptionPlanPricing
                {
                    Name = request.SubscriptionPlanId + "_" + request.CurrencyCode,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyId = request.CurrencyId,

                    EnableMonthlyPricingPlan = request.EnableMonthlyPricingPlan,
                    MonthlyAmount = request.MonthlyAmount,
                    MonthlyDiscount = request.MonthlyDiscount,
                    EnableYearlyPricingPlan = request.EnableYearlyPricingPlan,
                    YearlyAmount = request.YearlyAmount,
                    YearlyDiscount = request.YearlyDiscount,

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
                return Result.Failure($"Subscription pricing creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
