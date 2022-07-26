using FluentValidation;

namespace OnyxDoc.AuthService.Application.Users.Commands.UpdateUser
{
    public class EditInvitedUserCommandValidator : AbstractValidator<EditInvitedUserCommand>
    {
        public EditInvitedUserCommandValidator()
        {

            RuleFor(v => v.FirstName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
