using FluentValidation;
using Onyx.ContractService.Application.ContractDocuments.Commands.SaveSignedDocument;

namespace Onyx.ContractService.Application.Contract.Commands.SaveSignedDocument
{
    public class SaveSignedDocumentCommandValidator : AbstractValidator<SaveSignedDocumentCommand>
    {
        public SaveSignedDocumentCommandValidator()
        { 
            // RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            //RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");

            RuleFor(v => v.SigningAPIUrl).NotNull().NotEmpty().WithMessage("Signed API Url must be specified");
        }
    }
}
