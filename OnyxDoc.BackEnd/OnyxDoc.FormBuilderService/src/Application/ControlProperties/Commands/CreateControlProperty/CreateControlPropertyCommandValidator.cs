using FluentValidation;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    public class CreateControlPropertyCommandValidator : AbstractValidator<CreateControlPropertyCommand>
    {
        public CreateControlPropertyCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.ControlId).GreaterThan(0).WithMessage("Invalid control specified!");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Control property name must be specified!");
            RuleFor(v => v.DisplayName).NotEmpty().WithMessage("Control property display name must be specified!");
            RuleFor(v => v.ControlPropertyType).NotEmpty().WithMessage("Control property type must be specified!");
            RuleFor(v => v.ControlPropertyValueType).NotEmpty().WithMessage("Control property type value must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
