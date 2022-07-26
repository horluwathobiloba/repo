using FluentValidation; 

namespace Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiators
{
    public class UpdateContractTypeInitiatorsCommandValidator : AbstractValidator<UpdateContractTypeInitiatorsCommand>
    {
        public UpdateContractTypeInitiatorsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractTypeId).GreaterThan(0).WithMessage("A valid Contract type must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
