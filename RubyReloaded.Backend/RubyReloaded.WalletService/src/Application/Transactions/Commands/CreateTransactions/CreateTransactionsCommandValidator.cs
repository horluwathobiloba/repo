using FluentValidation;
using RubyReloaded.WalletService.Application.Transactions.Commands;

namespace RubyReloaded.WalletService.Application.Transactions
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionValidator()
        {
            RuleFor(v => v.TransactionType)
               .NotEmpty().NotNull();

            RuleFor(v => v.Amount)
               .NotEmpty().NotNull().WithMessage("Transaction Amount cannot be null");

            RuleFor(v => v.Narration)
               .NotEmpty().NotNull().WithMessage("Narration cannot be null");

            RuleFor(v => v.AccountId)
               .NotEmpty().NotNull().WithMessage("Account ID cannot be null");

            RuleFor(v => v.AccountNumber)
               .NotEmpty().NotNull().WithMessage("Account Number cannot be null");


            RuleFor(v => v.UserId)
                .NotEmpty().NotNull().WithMessage("UserID cannot be null");
        }
    }
}
