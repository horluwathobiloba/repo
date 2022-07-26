using FluentValidation;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    public class CreatePageControlItemCommandValidator : AbstractValidator<CreatePageControlItemCommand>
    {
        public CreatePageControlItemCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentPageId).GreaterThan(0).WithMessage("Invalid document page specified!");
            RuleFor(v => v.ControlId).GreaterThan(0).WithMessage("Invalid control specified!"); 

            RuleFor(v => v.Height).NotEmpty().WithMessage("Height value must be specified!");
            RuleFor(v => v.Width).NotEmpty().WithMessage("Width value must be specified!");
            RuleFor(v => v.Position).NotEmpty().WithMessage("Position value must be specified!");
            RuleFor(v => v.Transform).NotEmpty().WithMessage("Transform value must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
