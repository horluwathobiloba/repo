using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.ChangeUserAccessStatus
{
    public class ChangeUserAccessStatusCommandValidator:AbstractValidator<ChangeUserAccessStatusCommand>
    {
        public ChangeUserAccessStatusCommandValidator()
        {

            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.CooperativeId)
                .NotEmpty();

            RuleFor(v => v.CooperativeAccessStatus)
                .NotEmpty();
        }
    }
}
