using FluentValidation; 

namespace OnyxDoc.SubscriptionService.Application.Currencies.Commands
{
    public class CreateCurrenciesCommandValidator: AbstractValidator<CreateCurrenciesCommand>
    {
        public CreateCurrenciesCommandValidator()
        { 
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
