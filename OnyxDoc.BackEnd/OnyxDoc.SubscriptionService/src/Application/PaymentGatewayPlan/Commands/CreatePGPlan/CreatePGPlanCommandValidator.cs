using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class CreatePGPlanCommandValidator : AbstractValidator<CreatePGPlanCommand>
    {
        public CreatePGPlanCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Invalid subscription specified!");
            RuleFor(v => v.PaymentGatewayPlanId).NotEmpty().WithMessage("Invalid plan specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
