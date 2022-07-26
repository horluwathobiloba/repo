using FluentValidation;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using ReventInject;

namespace Onyx.ContractService.Application.WorkflowPhases.Commands.CreateWorkflowPhaseAndLevels
{
    public class CreateWorkflowPhaseAndLevelsCommandValidator : AbstractValidator<CreateWorkflowPhaseAndLevelsCommand>
    {
        private string error;
        public CreateWorkflowPhaseAndLevelsCommandValidator()
        {
            RuleFor(v => v.ContractTypeId).GreaterThan(0).WithMessage("ContractTypeId must be specified!");
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Workflow phase name must be specified!")
                .MaximumLength(200).WithMessage("Workflow phase name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.WorkflowSequence).IsInEnum().WithMessage("Workflow sequence must be specified!");
            RuleFor(v => v.WorkflowUserCategory).IsInEnum().WithMessage("Workflow user category must be specified!");

            RuleFor(v => v.WorkflowLevelRequests).Must(ValidateWorkflowLevels).WithMessage(error);
        }

        private bool ValidateWorkflowLevels(ICollection<CreateWorkflowLevelRequest> list)
        {
            if (list.Count <= 0)
            {
                error = "At least one workflow level is required for a workflow phase";
                return false;
            }
            var badRequests = (from x in list
                               where x.Rank <= 0 || !x.WorkflowLevelAction.IsEnum<WorkflowLevelAction>()
                               select x).ToList();
            if (badRequests.Count > 0)
            {
                error = $"{badRequests.Count} workflow level records have invalid 'Rank' or 'Workflow user category'.";
                return false;
            }
            return true;
        }
    }
}
