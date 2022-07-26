using FluentValidation;

namespace RubyReloaded.WalletService.Application.Products.Commands.CreateWishlist
{
    public class CreateWishlistValidator : AbstractValidator<CreateWishlistCommand>
    {
        public CreateWishlistValidator()
        {
            RuleFor(v => v.Name)
               .NotEmpty().NotNull().WithMessage("Please ");

            RuleFor(v => v.ProductId)
               .NotEmpty().NotNull().WithMessage("Invalid Product ID supplied");

            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("Invalid Product ID supplied");
        }
    }
}
