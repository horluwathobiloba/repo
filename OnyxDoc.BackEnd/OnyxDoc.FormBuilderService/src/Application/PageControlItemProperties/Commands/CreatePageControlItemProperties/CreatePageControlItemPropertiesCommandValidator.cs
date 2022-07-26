using FluentValidation;
using OnyxDoc.FormBuilderService.Application.Common.Models;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class CreatePageControlItemPropertiesCommandValidator : AbstractValidator<CreatePageControlItemPropertiesCommand>
    {
        public CreatePageControlItemPropertiesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PageControlItemId).GreaterThan(0).WithMessage("Invalid page control item identifier specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePageControlItemPropertyRequestValidator : AbstractValidator<CreatePageControlItemPropertyRequest>
    {
        public CreatePageControlItemPropertyRequestValidator()
        {
            RuleFor(v => v.ControlPropertyId).GreaterThan(0).WithMessage("Invalid control property identifier specified!");
        }
    }
}
