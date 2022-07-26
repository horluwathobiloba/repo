using FluentValidation;

namespace RubyReloaded.AuthService.Application.PermissionSets.Commands.ChangePermissionSet
{
    public class ChangePermissionSetStatusCommandValidator : AbstractValidator<ChangePermissionSetStatusCommand>
    {
        public ChangePermissionSetStatusCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.PermissionSetId)
              .NotEmpty();
        }
    }
}
