using FluentValidation;
using Onyx.ContractService.Application.WorkflowPhases.Commands.ChangeWorkflowPhaseStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.UpdateWorkflowPhaseStatus
{
    public class UpdateWorkflowPhaseStatusCommandValidator : AbstractValidator<UpdateWorkflowPhaseStatusCommand>
    {
        public UpdateWorkflowPhaseStatusCommandValidator()
        {
             RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).NotEqual(0).WithMessage("Workflow phase id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
