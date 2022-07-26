using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands
{
    class UpdatePageControlItemPropertyValueStatusValidator : AbstractValidator<UpdatePageControlItemPropertyValueStatusCommand>
    {
        public UpdatePageControlItemPropertyValueStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Page control item property value id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
