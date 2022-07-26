using FluentValidation;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class UpdatePageControlItemPropertyValuesCommandValidator : AbstractValidator<UpdatePageControlItemPropertyValuesCommand>
    {
        public UpdatePageControlItemPropertyValuesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PageControlItemPropertyId).GreaterThan(0).WithMessage("Page control item property identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class UpdatePageControlItemPropertyValueRequestValidator : AbstractValidator<UpdatePageControlItemPropertyValueRequest>
    {
        public UpdatePageControlItemPropertyValueRequestValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Page control item property value identifier must be specified!"); 
            RuleFor(v => v.Value).NotEmpty().WithMessage("Property value must be specified!"); 
        }
    }

}


