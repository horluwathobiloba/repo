using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.ChangeCustomerAccountStatus
{
    public class ChangeCustomerAccountStatusCommandValidator : AbstractValidator<ChangeCustomerAccountStatusCommand>
    {
        public ChangeCustomerAccountStatusCommandValidator()
        {
            RuleFor(v => v.CustomerAccountId).NotEmpty().NotNull();

            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}