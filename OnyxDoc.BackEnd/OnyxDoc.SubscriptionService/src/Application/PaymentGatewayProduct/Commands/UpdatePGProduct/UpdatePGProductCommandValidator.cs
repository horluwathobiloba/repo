using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class UpdatePGProductCommandValidator : AbstractValidator<UpdatePGProductCommand>
    {
        public UpdatePGProductCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PaymentGatewayProductId).NotEmpty().WithMessage("Product unique identifier must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


