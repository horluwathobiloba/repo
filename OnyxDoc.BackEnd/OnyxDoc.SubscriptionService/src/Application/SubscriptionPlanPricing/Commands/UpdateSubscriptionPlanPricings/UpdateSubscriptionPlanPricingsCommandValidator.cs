using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class UpdateSubscriptionPlanPricingsCommandValidator : AbstractValidator<UpdateSubscriptionPlanPricingsCommand>
    {
        public UpdateSubscriptionPlanPricingsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdateSubscriptionPlanPricingRequestValidator : AbstractValidator<UpdateSubscriptionPlanPricingRequest>
    {
        public UpdateSubscriptionPlanPricingRequestValidator()
        {
            RuleFor(v => v.CurrencyId).GreaterThan(0).WithMessage("Invalid currency identifier specified!");
            RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("Currency code must be specified!");
            RuleFor(v => v.Amount).GreaterThan(0).WithMessage("Amount must be specified because you enabled monthly pricing plan."); 
        }
    }
}


