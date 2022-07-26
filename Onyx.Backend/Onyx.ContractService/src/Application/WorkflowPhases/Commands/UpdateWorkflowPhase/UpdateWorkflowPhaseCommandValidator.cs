using FluentValidation;
using Onyx.ContractService.Application.WorkflowPhases.Commands.UpdateWorkflowPhase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.UpdateWorkflowPhase
{
    public class UpdateWorkflowPhaseCommandValidator : AbstractValidator<UpdateWorkflowPhaseCommand>
    {
        public UpdateWorkflowPhaseCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0);
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Workflow phase name must be specified!")
                .MaximumLength(200).WithMessage("Workflow phase name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.WorkflowSequence).IsInEnum().WithMessage("Workflow sequence must be specified!");
            RuleFor(v => v.WorkflowUserCategory).IsInEnum().WithMessage("Workflow user category must be specified!");
        }
    }
}
