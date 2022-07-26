using FluentValidation;
using Onyx.AuthService.Application.Users.Commands.UpdateUser;

namespace Onyx.AuthService.Application.RolePermissions.Commands.UpdateRoleAndPermissions
{
    public class UpdateRoleAndPermissionsCommandValidator : AbstractValidator<UpdateRoleAndPermissionsCommand>
    {
        public UpdateRoleAndPermissionsCommandValidator()
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
