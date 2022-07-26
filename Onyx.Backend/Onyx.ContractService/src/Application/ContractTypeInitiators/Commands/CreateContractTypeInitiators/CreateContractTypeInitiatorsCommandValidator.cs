using FluentValidation; 

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.CreateContractTypeInitiators
{
    public class CreateContractTypeInitiatorsCommandValidator: AbstractValidator<CreateContractTypeInitiatorsCommand>
    {
        public CreateContractTypeInitiatorsCommandValidator()
        { 
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractTypeId).GreaterThan(0).WithMessage("A valid Contract type Initiator must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
