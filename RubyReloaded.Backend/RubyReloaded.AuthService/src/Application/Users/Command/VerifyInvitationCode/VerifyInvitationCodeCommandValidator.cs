using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyInvitationCode
{
    public class VerifyInvitationCodeCommandValidator:AbstractValidator<VerifyInvitationCodeCommand>
    {
        public VerifyInvitationCodeCommandValidator()
        {
            RuleFor(v => v.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email cannot be empty");

            RuleFor(v => v.Code)
             .NotEmpty()
             .NotNull()
             .WithMessage("Code value cannot be empty"); 

            RuleFor(v => v.CooperativeId)
             .NotEmpty()
             .NotNull()
             .WithMessage("Cooperative value cannot be empty");
        }
    }
}
