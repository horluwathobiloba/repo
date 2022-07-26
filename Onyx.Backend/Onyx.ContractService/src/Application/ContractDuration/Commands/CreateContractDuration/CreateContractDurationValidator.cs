using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDuration
{
    public class CreateContractDurationValidator:AbstractValidator<CreateContractDurationCommand>
    {
        public CreateContractDurationValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Duration).GreaterThan(0).NotEmpty().NotNull().WithMessage("Duration Cannot Be Empty");
            RuleFor(v => v.DurationFrequency).NotEmpty().NotNull().WithMessage("DurationFrequency Cannot be empty");
        }
    }
}
