using FluentValidation; 

namespace Onyx.ContractService.Application.WorkflowLevels.Commands.CreateWorkflowLevels
{
    public class CreateWorkflowLevelsCommandValidator: AbstractValidator<CreateWorkflowLevelsCommand>
    {
        public CreateWorkflowLevelsCommandValidator()
        { 
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
