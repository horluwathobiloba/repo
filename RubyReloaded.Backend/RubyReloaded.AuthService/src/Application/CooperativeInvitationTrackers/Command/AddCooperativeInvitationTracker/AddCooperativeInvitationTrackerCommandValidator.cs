using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.CooperativeInvitationTrackers.Command.AddCooperativeInvitationTracker
{
    public class AddCooperativeInvitationTrackerCommandValidator: AbstractValidator<AddCooperativeInvitationTrackerCommand>
    {
        public AddCooperativeInvitationTrackerCommandValidator()
        {
            RuleFor(v => v.AdminEmail)
            .NotEmpty();

            RuleFor(v => v.CooperativeId)
            .NotEmpty();

            RuleFor(v => v.RequestType)
            .NotEmpty();
            RuleFor(v => v.UserEmail)
            .NotEmpty();
        }
    }
}
