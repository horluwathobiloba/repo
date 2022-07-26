using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoMembers.Command.ChangeAjoMemberStatus
{
    public class ChangeAjoMemberStatusCommandValidator:AbstractValidator<ChangeAjoMemberStatusCommand>
    {
        public ChangeAjoMemberStatusCommandValidator()
        {
            RuleFor(v => v.AjoMemberId)
           .NotEmpty();

            RuleFor(v => v.UserId)
         .NotEmpty();
        }
    }
}
