using FluentValidation;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Commands.UpdatePaymentChannel.UpdatePaymentChannelCommand
{
   public class UpdatePaymentmentChannelValidator:AbstractValidator<UpdatePaymentChannelCommand>
    {
        public UpdatePaymentmentChannelValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty().WithMessage("PaymentChannel name cannot be empty and cannot exceed 200 characters!");
            RuleFor(v => v.CurrencyConfigurationId).NotNull().NotEmpty().WithMessage("Currency code cannot be null!");
            RuleFor(v => v.TransactionFeeType.ToInt()).NotEqual(0).WithMessage("FeeType must be specified!");
            RuleFor(v => v.Id).NotEqual(0).WithMessage("FeeType must be specified!");
        }
    }
}
