using FluentValidation;
using System;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class ChangeSubscriptionCommandValidator : AbstractValidator<ChangeSubscriptionCommand>
    {
        public ChangeSubscriptionCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.NewSubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");
            RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("A valid currency code must be specified!");
            RuleFor(v => v.CurrencyId).GreaterThan(0).WithMessage("Invalid currency specified!");
            RuleFor(v => v.Amount).GreaterThanOrEqualTo(0).WithMessage("Invalid amount specified!");
            RuleFor(v => v.PaymentChannelId).GreaterThanOrEqualTo(0).WithMessage("Invalid payment channel specified!"); 
            RuleFor(v => v.Amount).GreaterThanOrEqualTo(0).WithMessage("Invalid amount specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }

    }
}
