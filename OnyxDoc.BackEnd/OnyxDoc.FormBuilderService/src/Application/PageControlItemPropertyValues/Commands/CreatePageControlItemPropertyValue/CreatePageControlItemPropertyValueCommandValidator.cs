using FluentValidation; 

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    public class CreatePageControlItemPropertyValueCommandValidator : AbstractValidator<CreatePageControlItemPropertyValueCommand>
    {       
        public CreatePageControlItemPropertyValueCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.PageControlItemPropertyId).GreaterThan(0).WithMessage("Page control item property identifier must be specified!");
            RuleFor(v => v.Value).NotEmpty().WithMessage("Property value must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
