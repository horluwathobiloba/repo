using FluentValidation;

namespace OnyxDoc.AuthService.Application.Roles.Commands.ChangeRole
{
    public class ChangeRoleStatusCommandValidator : AbstractValidator<ChangeRoleStatusCommand>
    {
        public ChangeRoleStatusCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
              .NotEmpty();
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.RoleId)
              .NotEmpty();
        }
    }
}
