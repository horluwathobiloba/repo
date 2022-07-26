using FluentValidation;
using Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevels
{
    public class UpdateWorkflowLevelsCommandValidator : AbstractValidator<UpdateWorkflowLevelsCommand>
    {
        public UpdateWorkflowLevelsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.WorkflowPhaseId).GreaterThan(0).WithMessage("A valid Workflow phase id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


