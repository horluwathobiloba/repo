using FluentValidation;
using ReventInject;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class UpdateStripeCardPaymentCommandValidator : AbstractValidator<UpdateStripeCardPaymentCommand>
    {
        public UpdateStripeCardPaymentCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEmpty().WithMessage("Subscriber id must be specified!");
            RuleFor(v => v.SubscriptionNo).NotEmpty().WithMessage("Subscription number id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");              
        }
    }
}


