using FluentValidation;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.Application.Controls.Commands
{
    public class UpdateControlCommandValidator : AbstractValidator<UpdateControlCommand>
    {
        public UpdateControlCommandValidator()
        {
            RuleFor(v => v.Id).NotEqual(0).WithMessage("Page control item property value id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name must be specified!"); 
            RuleFor(v => v.DisplayName).NotEmpty().WithMessage("Display name must be specified!");
            RuleFor(v => v.InputValueType).IsInEnum().WithMessage("Invalid input value type specified!");
            RuleFor(v => v.ControlType).IsInEnum().WithMessage("Invalid control type specified!");
            RuleFor(v => v.VersionNumber).GreaterThan(0).WithMessage("A valid version number must be specified!");

            When(v => v.ControlType == ControlType.BlockControl, () =>
            {
                RuleFor(v => v.BlockControlType).NotNull().WithMessage("Block control type must be specified!")
                .IsInEnum().WithMessage("Invalid block control type specified!");
            });
            When(v => v.ControlType == ControlType.FieldControl, () =>
            {
                RuleFor(v => v.FieldControlType).NotNull().WithMessage("Field control type must be specified!")
                .IsInEnum().WithMessage("Invalid field control type specified!");
            });

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }   
}


