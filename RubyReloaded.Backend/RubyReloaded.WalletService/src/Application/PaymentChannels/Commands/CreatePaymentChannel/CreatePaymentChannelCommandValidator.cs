using FluentValidation;
using ReventInject;
using RubyReloaded.WalletService.Application.PaymentChannels.Commands.CreatePaymentChannel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.PaymentChannels.Commands.CreatePaymentChannel
{
    public class CreatePaymentChannelCommandValidator : AbstractValidator<CreatePaymentChannelCommand>
    {
        public CreatePaymentChannelCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty().WithMessage("PaymentChannel name cannot be empty and cannot exceed 200 characters!");
            RuleFor(v => v.CurrencyId).NotNull().NotEmpty().WithMessage("Currency Id cannot be null!");
            
        }
    }
}