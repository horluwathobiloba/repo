using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.ActivateCustomerAccountStatus
{
    public class ActivateCustomerAccountStatusCommandValidator : AbstractValidator<ActivateCustomerAccountStatusCommand>
    {
        public ActivateCustomerAccountStatusCommandValidator()
        {
            RuleFor(v => v.CustomerAccountId).NotEmpty().NotNull();

            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}