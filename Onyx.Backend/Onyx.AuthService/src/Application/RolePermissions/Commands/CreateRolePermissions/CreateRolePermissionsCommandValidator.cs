using FluentValidation;
using Onyx.AuthService.Application.Roles.Commands.CreateRole;

namespace Onyx.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommandValidator : AbstractValidator<CreateRolePermissionsCommand>
    {
        public CreateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty()
              .NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty()
                .NotNull();

            //RuleFor(v => v.RoleId)
            //    .NotEmpty();

            //RuleFor(v => v.AccessLevel)
            //    .NotEmpty()
            //    .NotNull();
            
        }
    }
}
