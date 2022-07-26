using FluentValidation;
using Onyx.ContractService.Application.ReminderConfiguration.Commands;

namespace Onyx.ReminderService.Application.ReminderConfiguration.Commands.CreateReminderConfiguration
{
    public class CreateReminderConfigurationCommandValidator:AbstractValidator<CreateReminderConfigurationCommand>
    {
        public CreateReminderConfigurationCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotNull().WithMessage("A valid user Id must be specified!"); 
        }

    }
}
