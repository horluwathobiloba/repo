using FluentValidation;
using Onyx.ConractService.Application.ReminderConfigurations.Commands.UpdateReminderConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ReminderService.Application.ReminderConfigurations.Commands.UpdateReminderConfigurations
{
    public class UpdateReminderConfigurationsCommandValidator : AbstractValidator<UpdateReminderConfigurationsCommand>
    {
        public UpdateReminderConfigurationsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
