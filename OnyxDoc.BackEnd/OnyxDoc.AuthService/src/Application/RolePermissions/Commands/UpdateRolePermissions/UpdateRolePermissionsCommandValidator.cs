using FluentValidation;
using OnyxDoc.AuthService.Application.Users.Commands.UpdateUser;

namespace OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class UpdateRolePermissionsCommandValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.RoleAccessLevel)
             .NotEmpty()
             .NotNull();

            RuleFor(v => v.RoleId)
                .NotEmpty();
        }
    }
}
