using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Ajos.Command.ChangeAjoStatus
{
    public class ChangeAjoStatusCommandValidator:AbstractValidator<ChangeAjoStatusCommand>
    {
        public ChangeAjoStatusCommandValidator()
        {
            RuleFor(v => v.AjoId)
           .NotEmpty();
           
        }
    }
}
