using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.CreateWalletBeneficiary
{
    public class CreateWalletBeneficiaryCommandValidator: AbstractValidator<CreateWalletBeneficiaryCommand>
    {
        public CreateWalletBeneficiaryCommandValidator()
        {
            RuleFor(v => v.Username)
              .NotEmpty();

            RuleFor(v => v.Email)
               .NotEmpty();

            RuleFor(v => v.WalletId)
                .NotEmpty();
            RuleFor(v => v.PhoneNumber)
                .NotEmpty().NotNull();
        }
    }
}
