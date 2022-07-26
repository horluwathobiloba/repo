using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Bank.Commands.ChangeBankStatus
{
    internal class ChangeBankStatusCommandValidator : AbstractValidator<ChangeBankStatusCommand>
    {
        public ChangeBankStatusCommandValidator()
        {
            RuleFor(v => v.BankId) 
                .NotEmpty().NotNull(); 

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
