using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Notifications.Commands.ChangeNotoficationStatus
{
    public class ChangeNotificationStatusCommandValidator : AbstractValidator<ChangeNotificationStatusCommand>
    {
        public ChangeNotificationStatusCommandValidator()
        {
            RuleFor(v => v.Id)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input notification");

            RuleFor(v => v.DeviceId)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input Device Id");
        }
    }
}
