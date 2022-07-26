using FluentValidation;

namespace OnyxDoc.AuthService.Application.Users.Commands.VerifyEmail
{
    public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
         
            RuleFor(v => v.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Invalid Email");

        }
    }
}
