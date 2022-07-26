using FluentValidation; 

namespace OnyxDoc.DocumentService.Application.Document.Commands.DownloadDocument
{
    public class DownloadDocumentCommandValidator : AbstractValidator<DownloadDocumentCommand>
    {
        public DownloadDocumentCommandValidator()
        { 
             RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
