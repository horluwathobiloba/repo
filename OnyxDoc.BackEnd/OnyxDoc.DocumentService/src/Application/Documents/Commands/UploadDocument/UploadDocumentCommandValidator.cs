using FluentValidation; 

namespace OnyxDoc.DocumentService.Application.Document.Commands.UploadDocument
{
    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        public UploadDocumentCommandValidator()
        { 
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
