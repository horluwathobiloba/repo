using FluentValidation;
using RubyReloaded.AuthService.Application.RolePermissions.Commands.CreateRolePermissions;

namespace Ruby.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommandValidator : AbstractValidator<CreateRolePermissionsCommand>
    {
        public CreateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.CooperativeId)
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
