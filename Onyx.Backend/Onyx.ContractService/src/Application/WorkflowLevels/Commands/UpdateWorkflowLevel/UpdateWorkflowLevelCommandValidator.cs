using FluentValidation;
using Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevel
{
    public class UpdateWorkflowLevelCommandValidator : AbstractValidator<UpdateWorkflowLevelCommand>
    {
        public UpdateWorkflowLevelCommandValidator()
        { 
            RuleFor(v => v.Id).GreaterThan(0);
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.WorkflowPhaseId).GreaterThan(0).WithMessage("A valid Workflow phase id must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.RoleId).GreaterThan(0).WithMessage("A valid role id must be specified!");
            RuleFor(v => v.Rank).GreaterThan(0).WithMessage("Rank must be sepcified.");
            RuleFor(v => v.WorkflowLevelAction).IsInEnum().WithMessage("A valid Workflow level action must be specified!");           
        }
    }
}
