using FluentValidation;
using RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSsystemOwnerRoleAndPermission;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSystemOwnerRoleAndPermission
{
    public class CreateSystemOwnerRoleAndPermissionCommandValidator : AbstractValidator<CreateSystemOwnerRoleAndPermissionCommand>
    {
        public CreateSystemOwnerRoleAndPermissionCommandValidator()
        {

            RuleFor(v => v.UserId)
            .NotEmpty();
            RuleFor(v => v.AccessLevel)
          .NotEmpty();
            RuleFor(v => v.Name)
            .NotEmpty();
            RuleFor(v => v.SystemOwnerId)
            .NotEmpty();
        }
    }
}
