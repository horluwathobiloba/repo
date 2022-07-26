using FluentValidation; 
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.DeleteRecipient
{
    public class DeleteRecipientCommandValidator : AbstractValidator<DeleteRecipientCommand>
    {
        public DeleteRecipientCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage(" recipient must be specified!"); 
        }
    }
}
