using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.BillPayments.Commands.Validate
{
    public class ValidateCommandValidator:AbstractValidator<ValidateCommand>
    {
        public ValidateCommandValidator()
        {
            RuleFor(v => v.BillId)
            .NotEmpty();

            RuleFor(v => v.Inputs)
               .NotEmpty();
        }
    }
}
