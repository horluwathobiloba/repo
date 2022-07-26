using FluentValidation;
using OnyxDoc.DocumentService.Application.Documents.Commands.SendToDocumentSignatories;

namespace OnyxDoc.DocumentService.Application.Commands.SendToDocumentSignatories
{
    public class SendToDocumentSignatoriesCommandValidator : AbstractValidator<SendToDocumentSignatoriesCommand>
    {
        public SendToDocumentSignatoriesCommandValidator()
        { 
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.DocumentId).NotEqual(0).WithMessage("Document Id must be specified!");

            RuleFor(v => v.RecipientDetails).NotNull().NotEmpty().WithMessage("Invalid Recipients Specified");
        }
    }
}
