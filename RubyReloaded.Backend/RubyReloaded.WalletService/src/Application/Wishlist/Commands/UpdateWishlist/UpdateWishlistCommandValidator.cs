using FluentValidation;

namespace RubyReloaded.WalletService.Application.Products.Commands.UpdateWishlist
{
    public class UpdateWishlistValidator : AbstractValidator<UpdateWishlistCommand>
    {
        public UpdateWishlistValidator()
        {
            RuleFor(v => v.Name)
               .NotEmpty().NotNull();

            RuleFor(v => v.Id)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
