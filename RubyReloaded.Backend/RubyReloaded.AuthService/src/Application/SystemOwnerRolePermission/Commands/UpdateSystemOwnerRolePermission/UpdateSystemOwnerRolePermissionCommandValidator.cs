using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerRolePermission.Commands.UpdateSystemOwnerRolePermission
{
    public class UpdateSystemOwnerRolePermissionCommandValidator:AbstractValidator<UpdateSystemOwnerRolePermissionCommand>
    {
        public UpdateSystemOwnerRolePermissionCommandValidator()
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
