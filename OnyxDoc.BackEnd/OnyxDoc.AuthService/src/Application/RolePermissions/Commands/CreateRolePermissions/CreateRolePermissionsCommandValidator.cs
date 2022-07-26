using FluentValidation;
using OnyxDoc.AuthService.Application.Roles.Commands.CreateRole;

namespace OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommandValidator : AbstractValidator<CreateRolePermissionsCommand>
    {
        public CreateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
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
