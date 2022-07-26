using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.VirtualAccount.Commands.CreateVirtualAccount
{
    public class CreateVirtualAccountCommandValidator : AbstractValidator<CreateVirtualAccountCommand>
    {
        public CreateVirtualAccountCommandValidator()
        {
            RuleFor(v => v.VirtualAccountDtos)
                .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
