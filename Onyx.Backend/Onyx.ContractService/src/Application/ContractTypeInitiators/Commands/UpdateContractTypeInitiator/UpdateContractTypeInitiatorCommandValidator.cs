using FluentValidation;
using Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiator
{
    public class UpdateContractTypeInitiatorCommandValidator : AbstractValidator<UpdateContractTypeInitiatorCommand>
    {
        public UpdateContractTypeInitiatorCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Contract type Initiator id must be specified!");
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractTypeId).GreaterThan(0).WithMessage("A valid Contract type must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.RoleId).GreaterThan(0).WithMessage("A valid role must be specified!");
        }
    }
}
