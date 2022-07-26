using FluentValidation;

namespace RubyReloaded.SubscriptionService.Application.Currencies.Commands
{
    public class UpdateCurrenciesCommandValidator : AbstractValidator<UpdateCurrenciesCommand>
    {
        public UpdateCurrenciesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


