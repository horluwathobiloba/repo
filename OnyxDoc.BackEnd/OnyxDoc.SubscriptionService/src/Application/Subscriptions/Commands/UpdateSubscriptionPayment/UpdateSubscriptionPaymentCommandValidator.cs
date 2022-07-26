using FluentValidation;
using System;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class UpdateSubscriptionPaymentCommandValidator : AbstractValidator<UpdateSubscriptionPaymentCommand>
    {
        public UpdateSubscriptionPaymentCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");

            //RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("A valid currency code must be specified!");
            //RuleFor(v => v.CurrencyId).GreaterThan(0).WithMessage("Invalid currency specified!");
            RuleFor(v => v.Amount).GreaterThanOrEqualTo(0).WithMessage("Invalid amount specified!");
            RuleFor(v => v.PaymentChannelId).GreaterThanOrEqualTo(0).WithMessage("Invalid payment channel specified!");
            RuleFor(v => v.PaymentChannelId).GreaterThanOrEqualTo(0).WithMessage("Invalid amount specified!");
            RuleFor(v => v.Amount).GreaterThanOrEqualTo(0).WithMessage("Invalid amount specified!");

            RuleFor(v => v.PaymentChannelReference).NotEmpty().WithMessage("Payment channel reference must be specified!");
            RuleFor(v => v.PaymentChannelResponse).NotEmpty().WithMessage("Payment channel response must be specified!");
            RuleFor(v => v.PaymentChannelStatus).NotEmpty().WithMessage("Payment channel status must be specified!");
            RuleFor(v => v.TransactionReference).NotEmpty().WithMessage("Payment channel status must be specified!");

            RuleFor(v => v.PaymentStatus).IsInEnum().WithMessage("User id must be specified!");
            RuleFor(v => v.PaymentChannelStatus).NotEmpty().WithMessage("Payment channel status must be specified!");
            RuleFor(v => v.PaymentChannelStatus).NotEmpty().WithMessage("Payment channel status must be specified!");
            RuleFor(v => v.SubscriptionType).IsInEnum().WithMessage("Subscription type must be specified!");

            RuleFor(x => x.StartDate).Must(IsValidDate).WithMessage("Start date is required");
            RuleFor(x => x.EndDate).Must(IsValidDate).WithMessage("End date is required");
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(b => b.EndDate).WithMessage("End date is required");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");

        }

        private bool IsValidDate(DateTime date)
        {
            return !date.Equals(default);
        }
    }
}


