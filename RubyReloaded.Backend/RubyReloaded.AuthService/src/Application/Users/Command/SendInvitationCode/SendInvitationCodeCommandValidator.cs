using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.SendInvitationCode
{
    public class SendInvitationCodeCommandValidator:AbstractValidator<SendInvitationCodeCommand>
    {
        public SendInvitationCodeCommandValidator()
        {
            RuleFor(v => v.CooperativeId)
              .NotEmpty();

            RuleFor(v => v.RecipientEmail)
            .NotEmpty();
            
        }
    }
}
