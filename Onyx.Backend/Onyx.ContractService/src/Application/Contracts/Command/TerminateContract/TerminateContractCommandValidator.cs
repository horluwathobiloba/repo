using FluentValidation;

namespace Onyx.ContractService.Application.Contracts.Commands.TerminateContract
{
    public class TerminateContractCommandValidator : AbstractValidator<TerminateContractCommand>
    {
        public TerminateContractCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.TerminationReason).NotEmpty().WithMessage("Reason for termination must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!"); 
        }
    }
}
