using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Roles.Command.CreateRoleAndPermission
{
    public class CreateRoleAndPermissionCommandValidator:AbstractValidator<CreateRoleAndPermissionCommand>
    {
        public CreateRoleAndPermissionCommandValidator()
        {
            RuleFor(v => v.CooperativeId)
              .NotEmpty();

            //RuleFor(v => v.UserId)
            //    .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
