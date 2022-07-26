using FluentValidation; 

namespace Onyx.ContractService.Application.Contract.Commands.UploadSupportingDocument
{
    public class UploadSupportingDocumentValidator : AbstractValidator<UploadSupportingDocument>
    {
        public UploadSupportingDocumentValidator()
        { 
             RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");
            RuleFor(v => v.MimeType).NotEmpty().WithMessage("Mime Type must be specified!");
            RuleFor(v => v.File).NotEmpty().WithMessage("File must be specified!");
            RuleFor(v => v.Extension).NotEmpty().WithMessage("Extension must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
