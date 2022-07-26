using FluentValidation;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.CreateVirtualAccountConfig
{
    public class CreateWalletTransactionValidator : AbstractValidator<CreateWalletTransationCommand>
    {
        public CreateWalletTransactionValidator()
        {
            RuleFor(v => v.TransactionType)
               .NotEmpty().NotNull();

            RuleFor(v => v.TransactionAmount)
               .NotEmpty().NotNull();

            RuleFor(v => v.Balance)
               .NotEmpty().NotNull();

            RuleFor(v => v.Description)
               .NotEmpty().NotNull();

            RuleFor(v => v.WalletId)
               .NotEmpty().NotNull();

            RuleFor(v => v.WalletAccountNo)
               .NotEmpty().NotNull();

            RuleFor(v => v.TransactionStatus)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
