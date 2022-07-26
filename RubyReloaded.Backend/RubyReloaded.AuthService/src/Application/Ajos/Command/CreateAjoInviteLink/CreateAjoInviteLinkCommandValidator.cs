using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Ajos.Command.CreateAjoInviteLink
{
    public class CreateAjoInviteLinkCommandValidator:AbstractValidator<CreateAjoInviteLinkCommand>
    {
        public CreateAjoInviteLinkCommandValidator()
        {
         
            RuleFor(v => v.AjoId)
                .NotEmpty();

            RuleFor(v => v.RecipientEmails)
            .NotEmpty();

         
        }
    }
}
