using FluentValidation;
using OnyxDoc.DocumentService.Application.Documents.Commands.SaveSignedDocument;

namespace OnyxDoc.DocumentService.Application.Commands.SaveSignedDocument
{
    public class SaveSignedDocumentCommandValidator : AbstractValidator<SaveSignedDocumentCommand>
    {
        public SaveSignedDocumentCommandValidator()
        { 
           

            RuleFor(v => v.SigningAPIUrl).NotNull().NotEmpty().WithMessage("Signed API Url must be specified");
        }
    }
}
