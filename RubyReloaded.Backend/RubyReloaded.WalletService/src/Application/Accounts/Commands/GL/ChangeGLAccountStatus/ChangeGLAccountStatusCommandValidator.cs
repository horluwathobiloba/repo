using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
    public class ChangeGLAccountStatusCommandValidator : AbstractValidator<ChangeGLAccountStatusCommand>
    {
        public ChangeGLAccountStatusCommandValidator()
        {
            RuleFor(v => v.GLAccountId).NotEmpty().NotNull();

            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}