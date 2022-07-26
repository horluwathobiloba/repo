using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoInvitationTrackers.Commands.AddAjoInvitationTracker
{
    public class AddAjoInvitationTrackerCommandValidator:AbstractValidator<AddAjoInvitationTrackerCommand>
    {
        public AddAjoInvitationTrackerCommandValidator()
        {
            RuleFor(v => v.AdminEmail)
            .NotEmpty();

            RuleFor(v => v.AjoId)
            .NotEmpty();

            RuleFor(v => v.RequestType)
            .NotEmpty();
            RuleFor(v => v.UserEmail)
            .NotEmpty();
        }
    }
}
