using FluentValidation;
using OnyxDoc.AuthService.Application.Users.Commands.UpdateUser;

namespace OnyxDoc.AuthService.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
