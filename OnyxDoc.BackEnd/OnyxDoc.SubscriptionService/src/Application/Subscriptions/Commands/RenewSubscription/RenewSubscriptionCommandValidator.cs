using FluentValidation;
using System;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class RenewSubscriptionCommandValidator : AbstractValidator<RenewSubscriptionCommand>
    {
        public RenewSubscriptionCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }

        private bool IsValidDate(DateTime date)
        {
            return !date.Equals(default);
        }
    }
}
