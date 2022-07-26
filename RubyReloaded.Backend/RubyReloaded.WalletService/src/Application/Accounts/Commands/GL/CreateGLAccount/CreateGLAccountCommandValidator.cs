using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
    public class CreateGLAccountCommandValidator : AbstractValidator<CreateGLAccountCommand>
    {
        public CreateGLAccountCommandValidator()
        {
            RuleFor(v => v.Name) 
                .NotEmpty().NotNull().WithMessage("GLAccount Name must be specified");

            RuleFor(v => v.GLAccountType)
             .NotEmpty().NotNull().WithMessage("GLAccount Type must be specified");

            RuleFor(v => v.GLAccountClass)
            .NotEmpty().NotNull().WithMessage("GLAccount Class must be specified");

            RuleFor(v => v.UserId)
                .NotEmpty().NotNull().WithMessage("UserId must be specified"); ;
        }
    }
}
