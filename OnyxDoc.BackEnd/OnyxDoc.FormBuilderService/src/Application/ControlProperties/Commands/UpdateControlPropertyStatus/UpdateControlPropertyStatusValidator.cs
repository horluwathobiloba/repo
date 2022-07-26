using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Commands
{
    class UpdateControlPropertyStatusValidator : AbstractValidator<UpdateControlPropertyStatusCommand>
    {
        public UpdateControlPropertyStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.ControlId).GreaterThan(0).WithMessage("Control must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Control property identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User identifier must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
