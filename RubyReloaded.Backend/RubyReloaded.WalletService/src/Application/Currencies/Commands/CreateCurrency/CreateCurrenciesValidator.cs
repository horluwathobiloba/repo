

using FluentValidation;
using RubyReloaded.WalletService.Application.Currencys.Commands.CreateCurrencies;

namespace RubyReloaded.WalletService.Application.CurrencyConfigurations.Commands.CreateCurrenciess
{
    public class CreateCurrenciesValidator : AbstractValidator<CreateCurrenciesCommand>
    {
        public CreateCurrenciesValidator()
        {
            RuleFor(v => v.CurrencyCode)
                .NotEmpty();

        }
    }
}
