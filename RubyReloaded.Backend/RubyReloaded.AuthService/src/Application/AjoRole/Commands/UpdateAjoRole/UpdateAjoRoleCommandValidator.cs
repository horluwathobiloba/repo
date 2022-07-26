using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.UpdateAjoRole
{
    public class UpdateAjoRoleCommandValidator:AbstractValidator<UpdateAjoRoleCommand>
    {
        public UpdateAjoRoleCommandValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty();
        }
    }
}
