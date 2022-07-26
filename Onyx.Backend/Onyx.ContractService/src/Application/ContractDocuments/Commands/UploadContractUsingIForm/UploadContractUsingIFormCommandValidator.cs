using FluentValidation; 

namespace Onyx.ContractService.Application.Contract.Commands.UploadContractUsingIForm
{
    public class UploadContractUsingIFormCommandValidator : AbstractValidator<UploadContractUsingIFormCommand>
    {
        public UploadContractUsingIFormCommandValidator()
        { 
             RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
