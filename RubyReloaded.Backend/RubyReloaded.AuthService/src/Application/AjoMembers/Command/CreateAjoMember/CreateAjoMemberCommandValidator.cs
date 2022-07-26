using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoMembers.Command.CreateAjoMember
{
    public class CreateAjoMemberCommandValidator:AbstractValidator<CreateAjoMemberCommand>
    {
        public CreateAjoMemberCommandValidator()
        {
            RuleFor(v => v.Name)
           .NotEmpty();
            RuleFor(v => v.UserId)
             .NotEmpty();
            RuleFor(v => v.AjoId)
             .NotEmpty();
            RuleFor(v => v.Name)
             .MaximumLength(200)
              .NotEmpty();
        }
    }
}
