using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class UpdatePGSubscriptionsCommandValidator : AbstractValidator<UpdatePGSubscriptionsCommand>
    {
        public UpdatePGSubscriptionsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionId).NotEmpty().WithMessage("Subscription identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePGSubscriptionRequestValidator : AbstractValidator<UpdatePGSubscriptionRequest>
    {
        public UpdatePGSubscriptionRequestValidator()
        {
            RuleFor(v => v.PaymentGatewaySubscriptionCode).NotEmpty().WithMessage("Invalid subscription code specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
        }
    }
}



