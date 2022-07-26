using FluentValidation;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class UpdateDocumentPageCommandValidator : AbstractValidator<UpdateDocumentPageCommand>
    {
        public UpdateDocumentPageCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentId).GreaterThan(0).WithMessage("Invalid document specified!");
            RuleFor(v => v.PageIndex).GreaterThan(0).WithMessage("Invalid page index specified!");
            RuleFor(v => v.PageNumber).GreaterThan(0).WithMessage("Invalid page number specified!");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Document page name must be specified!");
            RuleFor(v => v.DisplayName).NotEmpty().WithMessage("Document page display name must be specified!");
            RuleFor(v => v.PageLayout).IsInEnum().WithMessage("Document page layout must be specified!");

            RuleFor(v => v.Height).NotEmpty().WithMessage("Document page height must be specified!");
            RuleFor(v => v.Width).NotEmpty().WithMessage("Document page width must be specified!");
            RuleFor(v => v.Position).NotEmpty().WithMessage("Document page position value must be specified!");
            RuleFor(v => v.Transform).NotEmpty().WithMessage("Document page transform value must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


