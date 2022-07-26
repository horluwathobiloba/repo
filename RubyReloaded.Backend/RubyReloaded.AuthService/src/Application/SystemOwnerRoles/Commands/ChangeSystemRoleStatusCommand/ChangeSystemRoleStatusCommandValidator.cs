using FluentValidation;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.ChangeSystemRoleStatusCommand
{
    public class ChangeSystemRoleStatusCommandValidator : AbstractValidator<ChangeSystemRoleStatusCommand>
    {
        public ChangeSystemRoleStatusCommandValidator()
        {
            RuleFor(v => v.RoleId)
            .NotEmpty();
            RuleFor(v => v.UserId)
            .NotEmpty();
        }
    }
}
