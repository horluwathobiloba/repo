using FluentValidation;
using Onyx.ContractService.Application.ContractTypes.Commands.ChangeContractTypeStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypes.Commands.FinalizeContractTypeConfiguration
{
    public class FinalizeContractTypeConfigurationCommandValidator : AbstractValidator<FinalizeContractTypeConfigurationCommand>
    {
        public FinalizeContractTypeConfigurationCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Contract type id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
