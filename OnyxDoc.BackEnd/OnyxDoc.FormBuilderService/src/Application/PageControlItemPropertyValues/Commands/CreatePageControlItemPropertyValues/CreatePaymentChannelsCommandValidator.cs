using FluentValidation;
using OnyxDoc.FormBuilderService.Application.Common.Models;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class CreatePageControlItemPropertyValuesCommandValidator: AbstractValidator<CreatePageControlItemPropertyValuesCommand>
    {
        public CreatePageControlItemPropertyValuesCommandValidator()
        { 
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.PageControlItemPropertyId).GreaterThan(0).WithMessage("Page control item property identifier must be specified!");
        }
    }

    public class CreatePageControlItemPropertyValueRequestValidator : AbstractValidator<CreatePageControlItemPropertyValueRequest>
    {
        public CreatePageControlItemPropertyValueRequestValidator()
        { 
            RuleFor(v => v.Name).NotEmpty().WithMessage("Page control item property value name must be specified!");
            RuleFor(v => v.Value).NotEmpty().WithMessage("Property value must be specified!");
        }
    }
}
