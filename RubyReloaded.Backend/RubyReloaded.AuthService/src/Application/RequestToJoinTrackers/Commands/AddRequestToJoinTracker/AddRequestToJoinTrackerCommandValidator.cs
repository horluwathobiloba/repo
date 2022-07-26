using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace RubyReloaded.AuthService.Application.RequestToJoinTrackers.Commands.AddRequestToJoinTracker
{
    public class AddRequestToJoinTrackerCommandValidator: AbstractValidator<AddRequestToJoinTrackerCommand>
    {
        public AddRequestToJoinTrackerCommandValidator()
        {
            RuleFor(v => v.AdminEmail)
             .NotEmpty();

            RuleFor(v => v.CooperativeId)
                .NotEmpty();
            RuleFor(v => v.UserEmail)
                .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
