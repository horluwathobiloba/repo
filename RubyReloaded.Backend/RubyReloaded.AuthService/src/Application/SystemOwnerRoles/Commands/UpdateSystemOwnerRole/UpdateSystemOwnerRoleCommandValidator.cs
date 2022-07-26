using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.UpdateSystemOwnerRole
{
    public class UpdateSystemOwnerRoleCommandValidator:AbstractValidator<UpdateSystemOwnerRoleCommand>
    {
        public UpdateSystemOwnerRoleCommandValidator()
        {
            RuleFor(v => v.RoleId)
        .NotEmpty();
            RuleFor(v => v.UserId)
            .NotEmpty();
            RuleFor(v => v.SystemOwnerId)
            .NotEmpty();
            RuleFor(v => v.AccessLevel)
            .NotEmpty();
        }
    }
}
