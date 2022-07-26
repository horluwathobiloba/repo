using FluentValidation;


namespace RubyReloaded.AuthService.Application.PermissionSets.Commands.UpdatePermissionSet
{
    public class UpdatePermissionSetCommandValidator : AbstractValidator<UpdatePermissionSetCommand>
    {
        public UpdatePermissionSetCommandValidator()
        {

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
