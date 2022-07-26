using FluentValidation;

namespace Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTag
{
    public class UpdateContractTagCommandValidator : AbstractValidator<UpdateContractTagCommand>
    {
        public UpdateContractTagCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("ContractTag must be specified!")
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


