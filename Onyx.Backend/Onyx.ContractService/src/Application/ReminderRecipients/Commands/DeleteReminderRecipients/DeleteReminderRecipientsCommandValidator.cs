using FluentValidation;
using Onyx.ContractService.Application.ReminderRecipients.Commands;


namespace Onyx.ReminderService.Application.ReminderRecipients.Commands.DeleteReminderRecipient
{
    public class DeleteReminderRecipientCommandValidator:AbstractValidator<DeleteReminderRecipientsCommand>
    {
        public DeleteReminderRecipientCommandValidator()
        {
           
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid reminder recipient id must be specified!");
        }

    }
}
