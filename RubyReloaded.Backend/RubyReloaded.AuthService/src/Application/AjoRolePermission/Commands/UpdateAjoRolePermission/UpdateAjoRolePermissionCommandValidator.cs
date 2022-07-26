using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoRolePermission.UpdateAjoRolePermission
{
    public class UpdateAjoRolePermissionCommandValidator:AbstractValidator<UpdateAjoRolePermissionCommand>
    {
        public UpdateAjoRolePermissionCommandValidator()
        {
            RuleFor(v => v.RolePermissions)
            .NotEmpty();

         

            //RuleFor(v => v.AccessLevel)
            // .NotEmpty()
            // .NotNull();

            RuleFor(v => v.RoleId)
                .NotEmpty();
        }
    }
}
