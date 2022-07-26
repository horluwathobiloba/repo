using FluentValidation;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.ContractTags.Commands.CreateContractTag;

namespace Onyx.ContractService.Application.ContractTags.Commands.CreateContractTags
{
    public class CreateContractTagCommandValidator : AbstractValidator<CreateContractTagCommand>
    {
        public CreateContractTagCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name must be specified!")
                .MaximumLength(200).WithMessage("Task Tag cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
