using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Notifications.Commands.UpdateNotification
{
    public class UpdateNotificationCommandValidator : AbstractValidator<UpdateNotificationCommand>
    {
        public UpdateNotificationCommandValidator()
        {
            RuleFor(v => v.NotificationStatus)
                 .NotEmpty();

            RuleFor(v => v.DeviceId)
              .NotEmpty()
              .NotNull();
        }
    }
}
