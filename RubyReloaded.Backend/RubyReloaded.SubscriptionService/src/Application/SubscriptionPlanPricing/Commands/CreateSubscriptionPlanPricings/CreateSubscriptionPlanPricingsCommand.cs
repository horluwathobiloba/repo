using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class CreateSubscriptionPlanPricingsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<CreateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionPlanPricingsCommandHandler : IRequestHandler<CreateSubscriptionPlanPricingsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionPlanPricingsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateSubscriptionPlanPricingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var list = new List<SubscriptionPlanPricing>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.SubscriptionPlanPricings)
                {
                    this.ValidateItem(item);
                    var exists = await _context.SubscriptionPlanPricings.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && a.CurrencyId == item.CurrencyId);

                    if (exists)
                    {
                        return Result.Failure($"Subscription pricing for '{item.CurrencyCode}' already exists.");
                    }
                    var entity = new SubscriptionPlanPricing
                    {

                        Name = request.SubscriptionPlanId + "_" + item.CurrencyCode,
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.SubscriptionPlanId,
                        CurrencyCode = item.CurrencyCode,
                        CurrencyId = item.CurrencyId,

                        EnableMonthlyPricingPlan = item.EnableMonthlyPricingPlan,
                        MonthlyAmount = item.MonthlyAmount,
                        MonthlyDiscount = item.MonthlyDiscount,
                        EnableYearlyPricingPlan = item.EnableYearlyPricingPlan,
                        YearlyAmount = item.YearlyAmount,
                        YearlyDiscount = item.YearlyDiscount,

                        UserId =  request.UserId,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.SubscriptionPlanPricings.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<SubscriptionPlanPricingDto>>(list);
                return Result.Success("Subscription pricings created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription pricings creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreateSubscriptionPlanPricingRequest item)
        {
            CreateSubscriptionCurrencyRequestValidator validator = new CreateSubscriptionCurrencyRequestValidator();

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
