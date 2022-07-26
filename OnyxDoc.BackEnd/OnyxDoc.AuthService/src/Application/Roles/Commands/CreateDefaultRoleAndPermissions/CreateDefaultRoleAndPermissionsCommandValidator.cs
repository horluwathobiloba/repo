using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Roles.Commands.CreateDefaultRoleAndPermissions
{
    internal class CreateDefaultRoleAndPermissionsCommandValidator : AbstractValidator<CreateDefaultRoleAndPermissionsCommand>
    {
        public CreateDefaultRoleAndPermissionsCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
             .NotEmpty();

            //RuleFor(v => v.UserId)
            //    .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
