using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDuration.Commands.UpdateContractDuration
{
    public class UpdateContractDurationCommandValidator:AbstractValidator<UpdateContractDurationCommand>
    {
        public UpdateContractDurationCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Duration).NotEmpty().NotNull().WithMessage("Duration Cannot Be Empty");
            RuleFor(v => v.DurationFrequency).NotEmpty().NotNull().WithMessage("DurationFrequency Cannot be empty");
        }
    }
}
