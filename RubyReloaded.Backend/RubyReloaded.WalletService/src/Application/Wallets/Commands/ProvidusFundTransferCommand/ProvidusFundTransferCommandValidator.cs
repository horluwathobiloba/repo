using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Wallets.Commands.ProvidusFundTransferCommand
{
    public class ProvidusFundTransferCommandValidator: AbstractValidator<ProvidusFundTransferCommand>
    {
        public ProvidusFundTransferCommandValidator()
        {

            RuleFor(v => v.CreditAccount)
               .NotEmpty();

            RuleFor(v => v.DebitAccount)
               .NotEmpty();

            RuleFor(v => v.TransactionAmount)
                .NotEmpty();
        

        }
    }
}
