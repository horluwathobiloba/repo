using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class UpdatePGPlansCommandValidator : AbstractValidator<UpdatePGPlansCommand>
    {
        public UpdatePGPlansCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePGPlanRequestValidator : AbstractValidator<UpdatePGPlanRequest>
    {
        public UpdatePGPlanRequestValidator()
        {
            RuleFor(v => v.SubscriptionId).NotEmpty().WithMessage("Subscription must be specified!");
            RuleFor(v => v.PaymentGatewayPlanId).NotEmpty().WithMessage("Invalid plan specified!");
            RuleFor(v => v.PaymentGatewayPlanCode).NotEmpty().WithMessage("Invalid plan code specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
        }
    }
}



