using FluentValidation;
using Onyx.AuthService.Application.Users.Commands.UpdateUser;

namespace Onyx.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class UpdateRolePermissionsCommandValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
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
