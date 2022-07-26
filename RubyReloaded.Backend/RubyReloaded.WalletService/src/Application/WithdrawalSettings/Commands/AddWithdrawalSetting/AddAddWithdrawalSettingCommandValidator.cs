using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.WithdrawalSettings.Commands
{
    public class AddAddWithdrawalSettingCommandValidator:AbstractValidator<AddWithdrawalSettingCommand>
    {
        public AddAddWithdrawalSettingCommandValidator()
        {
            RuleFor(v => v.BankName)
              .NotEmpty();
            RuleFor(v => v.UserId)
               .NotEmpty();
            RuleFor(v => v.FirstName)
                .NotEmpty();
            RuleFor(v => v.LastName)
                .NotEmpty().NotNull();
        }
    }
}
