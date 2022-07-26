using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.MakePayment.Commands
{
    public class MakePaymentCommandValidator : AbstractValidator<MakePaymentCommand>
    {
        public MakePaymentCommandValidator()
        {
            RuleFor(v => v.ApplicationType)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input notification");

            RuleFor(v => v.DeviceId)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input Device Id");
            RuleFor(v => v.UserId)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input User Id");
        }
    }
}
