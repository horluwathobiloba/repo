using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.ChangeCooperativeStatus
{
    public class ChangeCooperativeStatusCommandValidator : AbstractValidator<ChangeCooperativeStatusCommand>
    {
        public ChangeCooperativeStatusCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.CooperativeId)
           .NotEmpty();
        }
    }
}
