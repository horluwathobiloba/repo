using FluentValidation;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.PaymentChannels.Commands.UpdatePaymentChannel.UpdatePaymentChannelCommand
{
   public class UpdatePaymentmentChannelValidator:AbstractValidator<UpdatePaymentChannelCommand>
    {
        public UpdatePaymentmentChannelValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty().WithMessage("PaymentChannel name cannot be empty and cannot exceed 200 characters!");
            RuleFor(v => v.CurrencyId).NotNull().NotEmpty().WithMessage("Currency Id cannot be null!");
            RuleFor(v => v.Id).NotEqual(0).WithMessage("Payment Channel Id must be specified!");
        }
    }
}
