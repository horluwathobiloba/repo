﻿using FluentValidation; 

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class CreateSubscriptionPlanPricingCommandValidator : AbstractValidator<CreateSubscriptionPlanPricingCommand>
    {       
        public CreateSubscriptionPlanPricingCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!"); 
            RuleFor(v => v.CurrencyId).GreaterThan(0).WithMessage("Invalid currency specified!");
            RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("Currency code must be specified!");

            When(v => v.EnableMonthlyPricingPlan, () =>
            {
                RuleFor(v => v.MonthlyAmount).GreaterThan(0).WithMessage("Monthly amount must be specified because you enabled monthly pricing plan.");
                RuleFor(v => v.MonthlyDiscount).LessThan(0).WithMessage("Monthly discount must be greater than or equal to 0.");
            });

            When(v => v.EnableYearlyPricingPlan, () =>
            {
                RuleFor(v => v.MonthlyAmount).GreaterThan(0).WithMessage("Yearly amount must be specified because you enabled yearly pricing plan.");
                RuleFor(v => v.MonthlyDiscount).LessThan(0).WithMessage("Yearly discount must be greater than or equal to 0.");
            });

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
