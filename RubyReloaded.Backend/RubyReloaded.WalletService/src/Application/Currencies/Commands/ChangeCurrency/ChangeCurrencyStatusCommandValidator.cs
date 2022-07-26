using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Currencys.Commands.ChangeCurrency
{
    public class ChangeCurrencyStatusCommandValidator:AbstractValidator<ChangeCurrencyStatusCommand>
    {
        public ChangeCurrencyStatusCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
