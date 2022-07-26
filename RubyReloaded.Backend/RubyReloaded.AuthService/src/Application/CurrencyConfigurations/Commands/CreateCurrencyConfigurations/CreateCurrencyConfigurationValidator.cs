using FluentValidation;
using RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.CreateCurrencyConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.CreateCurrencyConfigurations
{
    public class CreateCurrencyConfigurationValidator : AbstractValidator<CreateCurrencyConfigurationCommand>
    {
        public CreateCurrencyConfigurationValidator()
        {
            RuleFor(v => v.CurrencyCode)
                .NotEmpty();

        }
    }
}
