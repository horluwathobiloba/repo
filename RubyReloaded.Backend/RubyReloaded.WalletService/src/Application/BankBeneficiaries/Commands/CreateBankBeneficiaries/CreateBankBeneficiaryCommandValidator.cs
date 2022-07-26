using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.CreateBankBeneficiaries
{
    public class CreateBankBeneficiaryCommandValidator : AbstractValidator<CreateBankBeneficiaryCommand>
    {
        public CreateBankBeneficiaryCommandValidator()
        {
            RuleFor(v => v.BankName)
              .NotEmpty();

            RuleFor(v => v.Email)
               .NotEmpty();

            RuleFor(v => v.AccountNumber)
                .NotEmpty();
            RuleFor(v => v.PhoneNumber)
                .NotEmpty()
                .NotNull();
        }
    }
}
