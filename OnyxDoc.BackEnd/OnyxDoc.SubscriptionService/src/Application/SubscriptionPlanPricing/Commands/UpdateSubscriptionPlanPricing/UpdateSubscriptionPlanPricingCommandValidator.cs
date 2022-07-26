using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class UpdateSubscriptionPlanPricingCommandValidator : AbstractValidator<UpdateSubscriptionPlanPricingCommand>
    {
        public UpdateSubscriptionPlanPricingCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");
            RuleFor(v => v.CurrencyId).NotEqual(0).WithMessage("Invalid Currency identifier specified!");
            RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("Currency code must be specified!");

            RuleFor(v => v.Amount).GreaterThan(0).WithMessage("Amount must be specified because you enabled monthly pricing plan.");
            RuleFor(v => v.Discount).LessThan(0).WithMessage("Discount must be greater than or equal to 0.");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


