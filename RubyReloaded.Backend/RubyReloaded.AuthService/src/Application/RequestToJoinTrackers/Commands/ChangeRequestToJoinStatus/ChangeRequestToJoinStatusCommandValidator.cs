using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.RequestToJoinTrackers.Commands.ChangeRequestToJoinStatus
{
    public class ChangeRequestToJoinStatusCommandValidator: AbstractValidator<ChangeRequestToJoinStatusCommand>
    {
        public ChangeRequestToJoinStatusCommandValidator()
        {
            RuleFor(v => v.RequestToJoinTrackerId)
                .NotEmpty();
            RuleFor(v => v.cooperativeAccessStatus)
                .NotEmpty();
        }
    }
}
