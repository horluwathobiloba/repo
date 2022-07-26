using FluentValidation;

namespace RubyReloaded.WalletService.Application.Wallets.Commands
{
    public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
    {
        public CreateWalletCommandValidator()
        {
            RuleFor(v => v.AccountName)
               .NotEmpty();

            RuleFor(v => v.Email)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.ProductCategory)
                .NotEmpty().NotNull();
        }
    }
}
