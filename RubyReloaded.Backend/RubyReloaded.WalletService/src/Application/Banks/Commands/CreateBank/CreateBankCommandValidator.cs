using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Bank.Commands.CreateBank
{
    public class CreateBankCommandValidator : AbstractValidator<CreateBankCommand>
    {
        public CreateBankCommandValidator()
        {
            RuleFor(v => v.BankDtos)
                .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty(); 
        }
    }
}
