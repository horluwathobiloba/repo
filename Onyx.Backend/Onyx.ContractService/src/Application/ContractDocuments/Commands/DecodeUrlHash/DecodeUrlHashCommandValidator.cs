using FluentValidation; 

namespace Onyx.ContractService.Application.Contract.Commands.DecodeUrlHash
{
    public class DecodeUrlHashCommandValidator : AbstractValidator<DecodeUrlHashCommand>
    {
        public DecodeUrlHashCommandValidator()
        { 
            // RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            //RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");

            RuleFor(v => v.DocumentLinkHash).NotEmpty().WithMessage("Document Link Hash must be specified!");
        }
    }
}
