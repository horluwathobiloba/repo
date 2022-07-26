using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoRolePermission.CreateAjoRolePermission
{
    public class CreateAjoRolePermmissionValidator:AbstractValidator<CreateAjoRolePermmission>
    {
        public CreateAjoRolePermmissionValidator()
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
