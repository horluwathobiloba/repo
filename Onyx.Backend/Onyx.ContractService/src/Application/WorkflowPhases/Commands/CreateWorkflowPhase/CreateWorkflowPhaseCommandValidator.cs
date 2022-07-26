using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.CreateWorkflowPhase
{
    public class CreateWorkflowPhaseCommandValidator : AbstractValidator<CreateWorkflowPhaseCommand>
    {
        public CreateWorkflowPhaseCommandValidator()
        {
            RuleFor(v => v.ContractTypeId).NotEqual(0).WithMessage("Organisation must be specified!");
             RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Workflow phase name must be specified!")
                .MaximumLength(200).WithMessage("Workflow phase name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.WorkflowSequence).IsInEnum().WithMessage("Workflow sequence must be specified!");
            RuleFor(v => v.WorkflowUserCategory).IsInEnum().WithMessage("Workflow user category must be specified!");
        }
    }
}
