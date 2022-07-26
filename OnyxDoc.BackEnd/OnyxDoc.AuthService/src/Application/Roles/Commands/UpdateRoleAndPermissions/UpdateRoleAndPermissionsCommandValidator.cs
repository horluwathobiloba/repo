using FluentValidation;
using OnyxDoc.AuthService.Application.Users.Commands.UpdateUser;

namespace OnyxDoc.AuthService.Application.RolePermissions.Commands.UpdateRoleAndPermissions
{
    public class UpdateRoleAndPermissionsCommandValidator : AbstractValidator<UpdateRoleAndPermissionsCommand>
    {
        public UpdateRoleAndPermissionsCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            //RuleFor(v => v.AccessLevel)
            // .NotEmpty()
            // .NotNull();

            RuleFor(v => v.RoleId)
                .NotEmpty();
        }
    }
}
