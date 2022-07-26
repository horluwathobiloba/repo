using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class UpdatePGPlanCommandValidator : AbstractValidator<UpdatePGPlanCommand>
    {
        public UpdatePGPlanCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PaymentGatewayPlanId).NotEmpty().WithMessage("Plan unique identifier must be specified!");
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Invalid subscription specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


