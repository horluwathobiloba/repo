﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class UpdateSubscriptionPlanPricingCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
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

    public class UpdateSubscriptionCurrencyCommandHandler : IRequestHandler<UpdateSubscriptionPlanPricingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateSubscriptionPlanPricingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var UpdatedEntityExists = await _context.SubscriptionPlanPricings
                       .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && x.CurrencyId == request.CurrencyId);

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"The currency named '{request.CurrencyCode}' is already configured for this subscription plan.");
                }

                var entity = await _context.SubscriptionPlanPricings.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.SubscriptionPlanId == request.SubscriptionPlanId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription currency specified.");
                }

                entity.CurrencyId = request.CurrencyId;
                entity.CurrencyCode = request.CurrencyCode;

                entity.EnableMonthlyPricingPlan = request.EnableMonthlyPricingPlan;
                entity.MonthlyAmount = request.MonthlyAmount;
                entity.MonthlyDiscount = request.MonthlyDiscount;
                entity.EnableYearlyPricingPlan = request.EnableYearlyPricingPlan;
                entity.YearlyAmount = request.YearlyAmount;
                entity.YearlyDiscount = request.YearlyDiscount;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.SubscriptionPlanPricings.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionPlanPricingDto>(entity);
                return Result.Success("Subscription pricing update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription pricing update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
