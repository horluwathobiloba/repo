using FluentValidation;
using OnyxDoc.DocumentService.Application.Document.Commands.UpdateDocumentName;

namespace OnyxDoc.DocumentService.Application.Document.Commands.UpdateDocument
{
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentNameCommand>
    {
        public UpdateDocumentCommandValidator()
        { 
             RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
