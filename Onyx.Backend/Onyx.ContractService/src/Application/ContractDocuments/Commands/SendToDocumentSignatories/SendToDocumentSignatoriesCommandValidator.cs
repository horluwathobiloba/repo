using FluentValidation;
using Onyx.ContractService.Application.ContractDocuments.Commands.SendToDocumentSignatories;

namespace Onyx.ContractService.Application.Contract.Commands.SendToDocumentSignatories
{
    public class SendToDocumentSignatoriesCommandValidator : AbstractValidator<SendToDocumentSignatoriesCommand>
    {
        public SendToDocumentSignatoriesCommandValidator()
        { 
             RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");

            RuleFor(v => v.RecipientDetails).NotNull().NotEmpty().WithMessage("Invalid Recipients Specified");
        }
    }
}
