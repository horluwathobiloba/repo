using FluentValidation;
using Onyx.ContractService.Application.ReminderRecipients.Commands;


namespace Onyx.ReminderService.Application.ReminderRecipients.Commands.CreateReminderRecipient
{
    public class CreateReminderRecipientCommandValidator:AbstractValidator<CreateReminderRecipientsCommand>
    {
        public CreateReminderRecipientCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract Id must be specified!");
            RuleFor(v => v.Email).NotNull().WithMessage("A valid email must be specified!"); 
        }

    }
}
