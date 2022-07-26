using FluentValidation;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    public class UpdateDocumentPageDimensionCommandValidator : AbstractValidator<UpdateDocumentPageDimensionCommand>
    {
        public UpdateDocumentPageDimensionCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentId).GreaterThan(0).WithMessage("Invalid document specified!");

            RuleFor(v => v.Height).NotEmpty().WithMessage("Document page height must be specified!");
            RuleFor(v => v.Width).NotEmpty().WithMessage("Document page width must be specified!");
            RuleFor(v => v.Position).NotEmpty().WithMessage("Document page position value must be specified!");
            RuleFor(v => v.Transform).NotEmpty().WithMessage("Document page transform value must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


