using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Currencies.Commands.CreateCurrenciesConfiguration
{
    public class CreateCurrenciesConfigurationValidator:AbstractValidator<CreateCurrenciesConfigurationCommand>
    {
        public CreateCurrenciesConfigurationValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Currencies).NotEmpty();
               
               
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
