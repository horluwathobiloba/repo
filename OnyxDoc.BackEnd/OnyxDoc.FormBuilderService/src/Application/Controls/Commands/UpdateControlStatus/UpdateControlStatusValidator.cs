using FluentValidation;
using OnyxDoc.FormBuilderService.Application.Controls.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertys.Commands
{
    class UpdateControlStatusValidator : AbstractValidator<UpdateControlStatusCommand>
    {
        public UpdateControlStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Document must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
