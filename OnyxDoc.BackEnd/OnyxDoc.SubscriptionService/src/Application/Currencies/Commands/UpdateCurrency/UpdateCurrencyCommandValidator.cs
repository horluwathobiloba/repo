using FluentValidation; 
namespace OnyxDoc.SubscriptionService.Application.Currencies.Commands
{
    public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.CurrencyCode).IsInEnum().WithMessage("Invalid Currency code specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


