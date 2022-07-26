using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class CreatePGSubscriptionCommandValidator : AbstractValidator<CreatePGSubscriptionCommand>
    {
        public CreatePGSubscriptionCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Invalid subscription identifier specified!");
            RuleFor(v => v.PaymentGatewaySubscriptionId).NotEmpty().WithMessage("Invalid subscription specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
