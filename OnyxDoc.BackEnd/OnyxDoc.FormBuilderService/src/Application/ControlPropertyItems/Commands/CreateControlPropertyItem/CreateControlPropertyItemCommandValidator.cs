using FluentValidation;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    public class CreateControlPropertyItemCommandValidator : AbstractValidator<CreateControlPropertyItemCommand>
    {
        public CreateControlPropertyItemCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.ControlPropertyId).GreaterThan(0).WithMessage("Invalid control property specified!"); 
            RuleFor(v => v.Index).GreaterThan(0).WithMessage("A valid control property item index must be specified!");
            RuleFor(v => v.Value).NotEmpty().WithMessage("Control property item value must be specified!"); 

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
