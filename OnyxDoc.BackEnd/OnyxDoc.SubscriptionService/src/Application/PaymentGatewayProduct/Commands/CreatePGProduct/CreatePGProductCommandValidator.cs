using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class CreatePGProductCommandValidator : AbstractValidator<CreatePGProductCommand>
    {
        public CreatePGProductCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");
            RuleFor(v => v.PaymentGatewayProductId).NotEmpty().WithMessage("Invalid product specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
