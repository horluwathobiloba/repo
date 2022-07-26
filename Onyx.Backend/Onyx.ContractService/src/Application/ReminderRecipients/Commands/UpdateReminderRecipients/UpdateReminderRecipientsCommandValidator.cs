using FluentValidation;
using Onyx.ConractService.Application.ReminderRecipients.Commands.UpdateReminderRecipients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ReminderService.Application.ReminderRecipients.Commands.UpdateReminderRecipients
{
    public class UpdateReminderRecipientsCommandValidator : AbstractValidator<UpdateReminderRecipientsCommand>
    {
        public UpdateReminderRecipientsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
