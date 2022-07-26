using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.ChangeCurrencyConfigurations
{
    public class ChangeCurrencyConfigurationStatusCommandValidator:AbstractValidator<ChangeCurrencyConfigurationStatusCommand>
    {
        public ChangeCurrencyConfigurationStatusCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
