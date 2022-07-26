using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class CreatePGSubscriptionsCommandValidator : AbstractValidator<CreatePGSubscriptionsCommand>
    {
        public CreatePGSubscriptionsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");      
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePGSubscriptionRequestValidator : AbstractValidator<CreatePGSubscriptionRequest>
    {
        public CreatePGSubscriptionRequestValidator()
        {
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Invalid subscription specified!");
            RuleFor(v => v.PaymentGatewaySubscriptionId).NotEmpty().WithMessage("Invalid payment gateway subscription unique identifier specified!");
            RuleFor(v => v.PaymentGatewaySubscriptionCode).NotEmpty().WithMessage("Invalid payment gateway subscription code identifier specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!"); 
        }
    }
}
