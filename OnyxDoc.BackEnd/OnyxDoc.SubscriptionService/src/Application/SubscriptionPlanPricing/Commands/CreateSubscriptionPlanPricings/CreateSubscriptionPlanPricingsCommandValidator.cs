using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class CreateSubscriptionPlanPricingsCommandValidator : AbstractValidator<CreateSubscriptionPlanPricingsCommand>
    {
        public CreateSubscriptionPlanPricingsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Subscription plan id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreateSubscriptionPlanPricingRequestValidator : AbstractValidator<CreateSubscriptionPlanPricingRequest>
    {
        public CreateSubscriptionPlanPricingRequestValidator()
        {
            RuleFor(v => v.CurrencyId).GreaterThan(0).WithMessage("Invalid currency specified!");
            RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("Currency code must be specified!");

            RuleFor(v => v.Amount).GreaterThan(0).WithMessage("Amount must be specified because you enabled monthly pricing plan.");
            RuleFor(v => v.Discount).LessThan(0).WithMessage("Discount must be greater than or equal to 0.");

        }
    }
}
