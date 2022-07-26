using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
    public class ActivateGLAccountStatusCommandValidator : AbstractValidator<ActivateGLAccountStatusCommand>
    {
        public ActivateGLAccountStatusCommandValidator()
        {
            RuleFor(v => v.GLAccountId).NotEmpty().NotNull();

            RuleFor(v => v.UserId).NotEmpty();
        }
    }
}