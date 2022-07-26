using FluentValidation;
using Onyx.ContractService.Application.WorkflowLevels.Commands.ChangeWorkflowLevelStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevelStatus
{
    public class UpdateWorkflowLevelStatusCommandValidator : AbstractValidator<UpdateWorkflowLevelStatusCommand>
    {
        public UpdateWorkflowLevelStatusCommandValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");

            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Workflow phase id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
