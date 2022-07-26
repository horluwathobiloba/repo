using FluentValidation; 

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class UpdatePageControlItemPropertyCommandValidator : AbstractValidator<UpdatePageControlItemPropertyCommand>
    {
        public UpdatePageControlItemPropertyCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PageControlItemId).GreaterThan(0).WithMessage("Invalid page control item identifier specified!");
            RuleFor(v => v.ControlPropertyId).GreaterThan(0).WithMessage("Invalid control property identifier specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


