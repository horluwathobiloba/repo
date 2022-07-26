using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Roles.Command.UpdateRoleAndPermission
{
    public class UpdateRoleAndPermissionsCommandValidator : AbstractValidator<UpdateRoleAndPermissionsCommand>
    {
        public UpdateRoleAndPermissionsCommandValidator()
        {
            RuleFor(v => v.CooperativeId)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.AccessLevel)
             .NotEmpty()
             .NotNull();

            RuleFor(v => v.RoleId)
                .NotEmpty();
        }
    }
}
