using FluentValidation;

namespace OnyxDoc.AuthService.Application.Roles.Commands.CreateRoleAndPermissions
{
    public class CreateRoleAndPermissionsCommandValidator : AbstractValidator<CreateRoleAndPermissionsCommand>
    {
        public CreateRoleAndPermissionsCommandValidator()
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
