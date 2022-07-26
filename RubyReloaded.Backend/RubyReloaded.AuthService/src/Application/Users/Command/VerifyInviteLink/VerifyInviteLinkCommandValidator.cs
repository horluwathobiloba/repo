using FluentValidation;
using RubyReloaded.AuthService.Application.User.Command.VerifyInviteLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyInviteLink
{
    public class VerifyInviteLinkCommandValidator:AbstractValidator<VerifyInviteLinkCommand>
    {
        public VerifyInviteLinkCommandValidator()
        {
            RuleFor(v => v.Token)
             .NotEmpty()
             .NotNull()
             .WithMessage("Token value cannot be empty");
        }
    }
}
