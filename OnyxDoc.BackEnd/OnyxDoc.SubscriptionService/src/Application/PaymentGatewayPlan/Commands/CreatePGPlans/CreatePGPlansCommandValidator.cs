using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    public class CreatePGPlansCommandValidator : AbstractValidator<CreatePGPlansCommand>
    {
        public CreatePGPlansCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");             
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePGPlanRequestValidator : AbstractValidator<CreatePGPlanRequest>
    {
        public CreatePGPlanRequestValidator()
        {
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");
            RuleFor(v => v.PaymentGatewayPlanCode).NotEmpty().WithMessage("Invalid plan code specified!");
            RuleFor(v => v.PaymentGatewayPlanId).NotEmpty().WithMessage("Invalid plan specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!"); 
        }
    }
}
