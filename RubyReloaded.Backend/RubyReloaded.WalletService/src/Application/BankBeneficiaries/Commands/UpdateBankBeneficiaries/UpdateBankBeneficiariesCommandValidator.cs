using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.UpdateBankBeneficiaries
{
    public class UpdateBankBeneficiariesCommandValidator:AbstractValidator<UpdateBankBeneficiariesCommand>
    {
        public UpdateBankBeneficiariesCommandValidator()
        {
            RuleFor(v => v.BankName)
              .NotEmpty();
            RuleFor(v => v.Id)
              .NotEmpty();
            RuleFor(v => v.LoggedInUser)
              .NotEmpty();
            RuleFor(v => v.Email)
               .NotEmpty();
            RuleFor(v => v.AccountNumber)
                .NotEmpty();
            RuleFor(v => v.PhoneNumber)
                .NotEmpty();
                
        }
    }
}
