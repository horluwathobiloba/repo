using FluentValidation;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Command.ChangeSuperAdmiUserStatus
{
    public class ChangeSystemOwnerUserStatusCommandValidator:AbstractValidator<ChangeSystemOwnerUserStatusCommand>
    {
        public ChangeSystemOwnerUserStatusCommandValidator()
        {
            RuleFor(v => v.UserId)
               .NotEmpty();
        }
    }
}
