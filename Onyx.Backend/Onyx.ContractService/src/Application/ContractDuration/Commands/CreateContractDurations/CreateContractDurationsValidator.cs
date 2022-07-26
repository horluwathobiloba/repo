using FluentValidation;
using Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDurations
{
    public class CreateContractDurationsValidator: AbstractValidator<CreateContractDurationsCommand>
    {
        public CreateContractDurationsValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Durations).NotNull().WithMessage("Contract duration request cannot be null");

        }
       
    }
}
