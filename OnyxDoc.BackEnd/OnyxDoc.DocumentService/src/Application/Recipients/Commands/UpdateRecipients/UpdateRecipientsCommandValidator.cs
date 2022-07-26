using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipients
{
    public class UpdateRecipientsCommandValidator : AbstractValidator<UpdateRecipientsCommand>
    {
        public UpdateRecipientsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
