using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerRolePermission.CreateSystemOwnerRolePermission
{
    public class CreateSystemOwnerRolePermissionCommandValidator:AbstractValidator<CreateSystemOwnerRolePermissionCommand>
    {
        public CreateSystemOwnerRolePermissionCommandValidator()
        {
            RuleFor(v => v.RoleId)
            .NotEmpty();

            RuleFor(v => v.UserId)
            .NotEmpty();

            RuleFor(v => v.RolePermissions)
            .NotEmpty();
        }

    }
}
