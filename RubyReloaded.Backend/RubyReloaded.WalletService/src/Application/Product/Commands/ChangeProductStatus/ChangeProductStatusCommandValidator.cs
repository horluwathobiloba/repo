using FluentValidation;

namespace RubyReloaded.WalletService.Application.Products.Commands.ChangeProductStatus
{
    public class ChangeProductStatusValidator : AbstractValidator<ChangeProductStatusCommand>
    {
        public ChangeProductStatusValidator()
        {
            RuleFor(v => v.ProductId)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
