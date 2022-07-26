using FluentValidation;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateContractStatus
{
    public class UpdateContractStatusCommandValidator : AbstractValidator<UpdateContractStatusCommand>
    {
        public UpdateContractStatusCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.ContractStatus).IsInEnum().WithMessage("Contract status must be specified!");
        }
    }
}
