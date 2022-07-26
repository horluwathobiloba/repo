using FluentValidation;
using RubyReloaded.AuthService.Application.PermissionSets.Commands.CreatePermissionSet;

namespace RubyReloaded.AuthService.Application.PermissionSets.Commands.CreatePermissionSet
{
    public class CreatePermissionSetCommandValidator : AbstractValidator<CreatePermissionSetCommand>
    {
        public CreatePermissionSetCommandValidator()
        {
          
            //RuleFor(v => v.UserId)
            //    .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
