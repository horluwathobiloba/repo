using FluentValidation;
using Onyx.ContractService.Application.ReminderConfigurations.Commands;


namespace Onyx.ReminderService.Application.ReminderConfigurations.Commands.DeleteReminderConfiguration
{
    public class DeleteReminderConfigurationCommandValidator:AbstractValidator<DeleteReminderConfigurationsCommand>
    {
        public DeleteReminderConfigurationCommandValidator()
        {
           
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid reminder recipient Id must be specified!");
        }

    }
}
