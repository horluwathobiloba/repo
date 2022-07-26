using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRoleAndPermissons
{
    public class CreateAjoRoleAndPermissonCommandValidator:AbstractValidator<CreateRoleAndPermissonAjoCommand>
    {
        public CreateAjoRoleAndPermissonCommandValidator()
        {
            RuleFor(x => x.AjoId)
                .NotNull()
                  .NotEmpty();
            RuleFor(x => x.Name)
                  .NotEmpty();
            //RuleFor(x => x.RolePermissions)
            //    .NotNull()
            //      .NotEmpty();
        }
    }
}
