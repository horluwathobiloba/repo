using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class CreatePGPriceCommandValidator : AbstractValidator<CreatePGPriceCommand>
    {
        public CreatePGPriceCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanPricingId).GreaterThan(0).WithMessage("Invalid subscription plan pricing specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
