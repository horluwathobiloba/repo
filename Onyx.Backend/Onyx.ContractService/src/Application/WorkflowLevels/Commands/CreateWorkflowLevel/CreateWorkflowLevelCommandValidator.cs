using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.CreateWorkflowLevel
{
    public class CreateWorkflowLevelCommandValidator:AbstractValidator<CreateWorkflowLevelCommand>
    {
        public CreateWorkflowLevelCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.WorkflowPhaseId).GreaterThan(0).WithMessage("Workflow phase id must be specified!");           
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Rank).GreaterThan(0).WithMessage("Rank must be specified for this Workflow level.");
            RuleFor(v => v.WorkflowLevelAction).IsInEnum().WithMessage("Workflow level action must be specified for each Workflow level!");
           // RuleFor(v => v.W).Must(AccountNo_Length).WithMessage("Account number Length must be equal or greater than 8");   
        }

        //private bool AccountNo_Length(string accountno)
        //{
        //    if (accountno.Length < 8)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}   
    }
}
