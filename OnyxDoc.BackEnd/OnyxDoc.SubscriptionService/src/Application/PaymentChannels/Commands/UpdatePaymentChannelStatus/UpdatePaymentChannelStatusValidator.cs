using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands
{
    class UpdatePaymentChannelStatusValidator : AbstractValidator<UpdatePaymentChannelStatusCommand>
    {
        public UpdatePaymentChannelStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("PaymentChannel id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
