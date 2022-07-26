using FluentValidation; 
namespace OnyxDoc.FormBuilderService.Application.Documents.Commands
{
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
    {
        public UpdateDocumentCommandValidator()
        {
            RuleFor(v => v.Id).NotEqual(0).WithMessage("Page control item property value id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name must be specified!"); 
            RuleFor(v => v.DocumentType).IsInEnum().WithMessage("Document type must be specified!");
            RuleFor(v => v.DocumentShareType).IsInEnum().WithMessage("Document share type must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }   
}


