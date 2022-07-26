using FluentValidation;

namespace RubyReloaded.WalletService.Application.Products.Commands.ExtendWishlist
{
    public class ExtendWishlistValidator : AbstractValidator<ExtendWishlistCommand>
    {
        public ExtendWishlistValidator()
        {
            RuleFor(v => v.WishlistId)
               .NotEmpty().NotNull().WithMessage("Invalid Wishlist Id");
            RuleFor(v => v.WishlistExtensionAmount)
               .NotEmpty().NotNull().WithMessage("Please provide Wishlist Extension Amount");
            RuleFor(v => v.WishlistExtensionDate)
              .NotEmpty().NotNull().WithMessage("Please provide Wishlist Extension Date");
          

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
