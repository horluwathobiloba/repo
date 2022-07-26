using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.DeactivateCustomerAccountStatus
{
    public class DeactivateCustomerAccountStatusCommandValidator : AbstractValidator<DeactivateCustomerAccountStatusCommand>
    {
        public DeactivateCustomerAccountStatusCommandValidator()
        {
            RuleFor(v => v.CustomerAccountId).NotEmpty().NotNull();

            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}