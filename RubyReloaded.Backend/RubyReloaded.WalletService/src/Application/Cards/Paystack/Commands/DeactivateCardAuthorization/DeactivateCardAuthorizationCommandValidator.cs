using FluentValidation;

namespace RubyReloaded.WalletService.Application.Cards.Commands.DeactivateCardAuthorization
{
    public class DeactivateCardAuthorizationValidator : AbstractValidator<DeactivateCardAuthorizationCommand>
    {
        public DeactivateCardAuthorizationValidator()
        {
            RuleFor(v => v.AuthorizationCode)
               .NotEmpty().NotNull().WithMessage("Please input valid Authorization Code");

            RuleFor(v => v.UserId)
               .NotEmpty().NotNull().WithMessage("Please input valid User Id");

        }
    }
}
