using FluentValidation;
using OnyxDoc.FormBuilderService.Application.Common.Models;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    public class UpdatePageControlItemsCommandValidator : AbstractValidator<UpdatePageControlItemsCommand>
    {
        public UpdatePageControlItemsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentPageId).GreaterThan(0).WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePageControlItemRequestValidator : AbstractValidator<UpdatePageControlItemRequest>
    {
        public UpdatePageControlItemRequestValidator()
        {
            RuleFor(v => v.ControlId).GreaterThan(0).WithMessage("Invalid Control specified!");
            RuleFor(v => v.Height).NotEmpty().WithMessage("Height value must be specified!");
            RuleFor(v => v.Width).NotEmpty().WithMessage("Width value must be specified!");
            RuleFor(v => v.Position).NotEmpty().WithMessage("Position value must be specified!");
            RuleFor(v => v.Transform).NotEmpty().WithMessage("Transform value must be specified!");
        }
    }
}


