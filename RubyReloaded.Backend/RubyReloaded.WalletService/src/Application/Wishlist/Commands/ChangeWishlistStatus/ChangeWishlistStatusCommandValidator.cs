using FluentValidation;

namespace RubyReloaded.WalletService.Application.Services.Commands.ChangeWishlistStatus
{
    public class ChangeWishlistStatusValidator : AbstractValidator<ChangeWishlistStatusCommand>
    {
        public ChangeWishlistStatusValidator()
        {
            RuleFor(v => v.Id)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
