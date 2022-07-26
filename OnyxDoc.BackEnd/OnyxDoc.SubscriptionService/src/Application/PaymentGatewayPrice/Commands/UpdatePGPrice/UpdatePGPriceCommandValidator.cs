using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class UpdatePGPriceCommandValidator : AbstractValidator<UpdatePGPriceCommand>
    {
        public UpdatePGPriceCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanPricingId).GreaterThan(0).WithMessage("Invalid subscription plan pricing specified!");
            RuleFor(v => v.PaymentGatewayPriceId).NotEmpty().WithMessage("Price unique identifier must be specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


