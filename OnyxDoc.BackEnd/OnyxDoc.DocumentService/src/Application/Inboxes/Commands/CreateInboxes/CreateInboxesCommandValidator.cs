using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Inboxes.Commands.CreateInboxes
{
    public class CreateInboxesCommandValidator:AbstractValidator<CreateInboxesCommand>
    {
        public CreateInboxesCommandValidator()
        {
            // RuleFor(v => v.).NotEqual(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.InboxVms).NotEmpty();
          
        }
    }
}
