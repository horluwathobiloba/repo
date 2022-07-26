using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Notifications.Commands.CreateNotification
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(v => v.Message)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input notification");

            RuleFor(v => v.DeviceId)
                 .NotNull()
                 .NotEmpty().WithMessage("Please input Device Id");
        }
    }
}
