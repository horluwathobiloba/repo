using FluentValidation; 

namespace RubyReloaded.SubscriptionService.Application.Currencies.Commands
{
    public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
    {       
        public CreateCurrencyCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!");            
            RuleFor(v => v.CurrencyCode).IsInEnum().WithMessage("Invalid Currency code specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
