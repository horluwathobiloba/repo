using FluentValidation;
using OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipient;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipient
{
    public class UpdateRecipientCommandValidator : AbstractValidator<UpdateRecipientCommand>
    {
        public UpdateRecipientCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage(" recipient must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid  must be specified!");
            RuleFor(v => v.Email).NotEmpty().WithMessage("A valid email must be specified!");
            RuleFor(v => v.RecipientCategory).IsInEnum().WithMessage("A valid recipient category must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
