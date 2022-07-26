using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoRolePermission.Queries.GetAjoRolePermissions
{
    public class GetAjoRolePermmisionByRoleIdCommandValidator : AbstractValidator<GetAjoRolePermmisionByRoleIdCommand>
    {
        public GetAjoRolePermmisionByRoleIdCommandValidator()
        {
            RuleFor(v => v.RoleId)
            .NotEmpty();
           
        }
    }
}
