using FluentValidation;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipient
{
    public class CreateRecipientCommandValidator:AbstractValidator<CreateRecipientCommand>
    {
        public CreateRecipientCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.RecipientCategory).IsInEnum().WithMessage("A valid recipient category must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!"); 
        }

    }
}
