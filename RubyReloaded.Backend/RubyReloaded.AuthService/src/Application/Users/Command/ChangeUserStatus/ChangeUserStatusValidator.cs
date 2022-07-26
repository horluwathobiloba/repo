using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.User.Command.ChangeUserStatus
{
    public class ChangeUserStatusValidator: AbstractValidator<ChangeUserStatusCommand>
    {
        public ChangeUserStatusValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
