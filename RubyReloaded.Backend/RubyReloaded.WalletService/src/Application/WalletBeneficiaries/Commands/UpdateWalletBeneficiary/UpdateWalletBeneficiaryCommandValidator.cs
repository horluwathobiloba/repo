using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.UpdateWalletBeneficiary
{
    public class UpdateWalletBeneficiaryCommandValidator : AbstractValidator<UpdateWalletBeneficiaryCommand>
    {
        public UpdateWalletBeneficiaryCommandValidator()
        {
            RuleFor(v => v.LoggedInUser)
             .NotEmpty();
            RuleFor(v => v.Id)
             .NotEmpty();
            RuleFor(v => v.Email)
               .NotEmpty();
            RuleFor(v => v.Username)
                 .NotEmpty();
            RuleFor(v => v.PhoneNumber)
                .NotEmpty()
                .NotNull();
        }
    }
}
