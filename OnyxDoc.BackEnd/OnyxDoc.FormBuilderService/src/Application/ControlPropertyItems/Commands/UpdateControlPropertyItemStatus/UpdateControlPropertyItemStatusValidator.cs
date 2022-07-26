using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands
{
    class UpdateControlPropertyItemStatusValidator : AbstractValidator<UpdateControlPropertyItemStatusCommand>
    {
        public UpdateControlPropertyItemStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.ControlPropertyId).GreaterThan(0).WithMessage("Document must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Document page id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
