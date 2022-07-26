using FluentValidation; 

namespace Onyx.ContractService.Application.Contract.Commands.UploadContract
{
    public class UploadContractCommandValidator : AbstractValidator<UploadContractCommand>
    {
        public UploadContractCommandValidator()
        { 
             RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
