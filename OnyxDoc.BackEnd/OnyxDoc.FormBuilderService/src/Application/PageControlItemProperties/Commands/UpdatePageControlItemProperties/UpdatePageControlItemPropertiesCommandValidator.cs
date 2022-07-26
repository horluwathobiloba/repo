using FluentValidation;
using OnyxDoc.FormBuilderService.Application.Common.Models;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands
{
    public class UpdatePageControlItemPropertiesCommandValidator : AbstractValidator<UpdatePageControlItemPropertiesCommand>
    {
        public UpdatePageControlItemPropertiesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PageControlItemId).GreaterThan(0).WithMessage("Page control item must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePageControlItemPropertyRequestValidator : AbstractValidator<UpdatePageControlItemPropertyRequest>
    {
        public UpdatePageControlItemPropertyRequestValidator()
        {
            RuleFor(v => v.ControlPropertyId).GreaterThan(0).WithMessage("Invalid control property specified!");  
        }
    }
}


