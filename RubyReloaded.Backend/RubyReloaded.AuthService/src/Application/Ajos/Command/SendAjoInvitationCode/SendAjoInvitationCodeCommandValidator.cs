using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Ajos.Command.SendAjoInvitationCode
{
   public class SendAjoInvitationCodeCommandValidator:AbstractValidator<SendAjoInvitationCodeCommand>
   {
        public SendAjoInvitationCodeCommandValidator()
        {
            RuleFor(v => v.AjoId)
              .NotEmpty();

            RuleFor(v => v.RecipientEmails)
            .NotEmpty();

            
        }
   }
}
