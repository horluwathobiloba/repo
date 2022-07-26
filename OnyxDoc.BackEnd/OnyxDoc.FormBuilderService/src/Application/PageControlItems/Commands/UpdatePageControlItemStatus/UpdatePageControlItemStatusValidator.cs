using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Commands
{
    class UpdatePageControlItemStatusValidator : AbstractValidator<UpdatePageControlItemStatusCommand>
    {
        public UpdatePageControlItemStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentPageId).GreaterThan(0).WithMessage("Document page identifier must be specified!");
            RuleFor(v => v.ControlId).GreaterThan(0).WithMessage("Control identifier must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Page control item identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
