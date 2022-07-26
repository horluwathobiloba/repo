using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.ChangeUserNotificationStatus
{
    public class ChangeNotificationStatusCommandValidator:AbstractValidator<ChangeNotificationStatusCommand>
    {
        public ChangeNotificationStatusCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
