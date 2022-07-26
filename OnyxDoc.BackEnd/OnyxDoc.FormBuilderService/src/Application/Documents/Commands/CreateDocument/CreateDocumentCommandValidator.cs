using FluentValidation;

namespace OnyxDoc.FormBuilderService.Application.Documents.Commands
{
    public class CreateDocumentCommandValidator : AbstractValidator<CreateDocumentCommand>
    {
        public CreateDocumentCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name must be specified!");
            RuleFor(v => v.DocumentType).IsInEnum().WithMessage("Document type must be specified!");
            RuleFor(v => v.DocumentShareType).IsInEnum().WithMessage("Document share type must be specified!");                    

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
