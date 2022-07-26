using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using FluentValidation.Results;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class UpdateSubscriptionPlanPricingsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<UpdateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionPlanPricingsCommandHandler : IRequestHandler<UpdateSubscriptionPlanPricingsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateSubscriptionPlanPricingsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateSubscriptionPlanPricingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);
                var list = new List<SubscriptionPlanPricing>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.SubscriptionPlanPricings)
                {
                    this.ValidateItem(item);

                    //check if the name of the vendor type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.SubscriptionPlanPricings
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && x.CurrencyId == item.CurrencyId);

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"The currency code '{item.CurrencyCode}' is already configured for this subscription.");
                    }

                    var entity = await _context.SubscriptionPlanPricings
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && (x.Id == item.Id || x.CurrencyId == item.CurrencyId || x.CurrencyCode == item.CurrencyCode))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new SubscriptionPlanPricing
                        {
                            Name = item.CurrencyCode,
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            SubscriptionPlanId = request.SubscriptionPlanId,
                            CurrencyId = item.CurrencyId,
                            CurrencyCode = item.CurrencyCode,

                            EnableMonthlyPricingPlan = item.EnableMonthlyPricingPlan,
                            MonthlyAmount = item.MonthlyAmount,
                            MonthlyDiscount = item.MonthlyDiscount,
                            EnableYearlyPricingPlan = item.EnableYearlyPricingPlan,
                            YearlyAmount = item.YearlyAmount,
                            YearlyDiscount = item.YearlyDiscount,

                            UserId = request.UserId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.CurrencyId = item.CurrencyId;
                        entity.CurrencyCode = item.CurrencyCode;

                        entity.EnableMonthlyPricingPlan = item.EnableMonthlyPricingPlan;
                        entity.MonthlyAmount = item.MonthlyAmount;
                        entity.MonthlyDiscount = item.MonthlyDiscount;
                        entity.EnableYearlyPricingPlan = item.EnableYearlyPricingPlan;
                        entity.YearlyAmount = item.YearlyAmount;
                        entity.YearlyDiscount = item.YearlyDiscount;
                        entity.Status = item.Status;

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.SubscriptionPlanPricings.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<SubscriptionPlanPricingDto>>(list);
                return Result.Success("Subscription pricings update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription pricing update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateSubscriptionPlanPricingRequest item)
        {
            UpdateSubscriptionCurrencyRequestValidator validator = new UpdateSubscriptionCurrencyRequestValidator();

            ValidationResult validateResult = validator.Validate(item);
            string validateError = null;

            if (!validateResult.IsValid)
            {
                foreach (var failure in validateResult.Errors)
                {
                    validateError += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage + "\n";
                }
                throw new Exception(validateError);
            }
        }


    }


}
