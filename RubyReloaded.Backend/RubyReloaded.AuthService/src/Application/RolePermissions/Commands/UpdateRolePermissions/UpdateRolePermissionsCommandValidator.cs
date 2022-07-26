using FluentValidation;

namespace RubyReloaded.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class UpdateRolePermissionsCommandValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.CooperativeId)
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
