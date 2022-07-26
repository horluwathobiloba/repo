using FluentValidation;
using OnyxDoc.AuthService.Application.Roless.Commands.CreateRoles;

namespace OnyxDoc.AuthService.Application.Roles.Commands.CreateRoles
{
    public class CreateRolesCommandValidator : AbstractValidator<CreateRolesCommand>
    {
        public CreateRolesCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
              .NotEmpty();

            //RuleFor(v => v.UserId)
            //    .NotEmpty();

            //RuleFor(v => v.RolesVm)
            //    .MaximumLength(200)
            //    .NotEmpty();
        }
    }
}
