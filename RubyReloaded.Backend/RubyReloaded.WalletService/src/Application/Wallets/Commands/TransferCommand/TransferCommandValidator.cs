using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
namespace RubyReloaded.WalletService.Application.Wallets.Commands.CommandTransfer
{
    public class TransferCommandValidator : AbstractValidator<TransferCommand>
    {
        public TransferCommandValidator()
        {
            RuleFor(v => v.UserId)
               .NotEmpty();

            RuleFor(v => v.beneficiaryAccountName)
               .NotEmpty();

            RuleFor(v => v.beneficiaryAccountNumber)
                .NotEmpty();
            RuleFor(v => v.transactionAmount)
                .NotEmpty();

            RuleFor(v => v.ReciepientName)
               .NotEmpty();
        }
    }
}
