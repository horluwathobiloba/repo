using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.VirtualAccount.Commands.ChangeVirtualAccount
{
    public class ChangeVirtualAccountStatusCommandValidator : AbstractValidator<ChangeVirtualAccountStatusCommand>
    {
        public ChangeVirtualAccountStatusCommandValidator()
        {
            RuleFor(v => v.VirtualAccountId).NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}



