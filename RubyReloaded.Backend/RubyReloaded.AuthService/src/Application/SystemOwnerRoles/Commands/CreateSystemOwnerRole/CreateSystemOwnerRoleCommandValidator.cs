using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSystemOwnerRole
{
    public class CreateSystemOwnerRoleCommandValidator:AbstractValidator<CreateSystemOwnerRoleCommand>
    {
        public CreateSystemOwnerRoleCommandValidator()
        {
          //  RuleFor(v => v.RoleId)
          //.NotEmpty();
            RuleFor(v => v.UserId)
            .NotEmpty();
          //  RuleFor(v => v.AccessLevel)
          //.NotEmpty();
            RuleFor(v => v.Name)
            .NotEmpty();
            RuleFor(v => v.SystemOwnerId)
            .NotEmpty();
        }
    }
}
