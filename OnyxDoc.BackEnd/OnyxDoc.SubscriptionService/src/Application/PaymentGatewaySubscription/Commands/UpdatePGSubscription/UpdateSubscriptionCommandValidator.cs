using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class UpdatePGSubscriptionCommandValidator : AbstractValidator<UpdatePGSubscriptionCommand>
    {
        public UpdatePGSubscriptionCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PaymentGatewaySubscriptionCode).NotEmpty().WithMessage("Subscription code must be specified!");
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Invalid subscription specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


