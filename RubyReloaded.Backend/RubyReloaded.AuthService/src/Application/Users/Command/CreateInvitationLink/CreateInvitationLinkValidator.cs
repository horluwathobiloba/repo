using FluentValidation;
using RubyReloaded.AuthService.Application.User.Command.GetInvitationLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.User.Command.CreateInvitationLink
{
    public class CreateInvitationLinkValidator: AbstractValidator<CreateInvitationLinkCommand>
    {
        public CreateInvitationLinkValidator()
        {
         

            RuleFor(v => v.CooperativeId)
                .NotEmpty();

            RuleFor(v => v.RecipientEmail)
            .NotEmpty();

            
        }
    }
}
