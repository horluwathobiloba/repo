using FluentValidation;

namespace OnyxDoc.AuthService.Application.Commands.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .MaximumLength(200)
                .NotEmpty()
                .WithMessage("Invalid First name ");

            RuleFor(v => v.Email)
              .MaximumLength(200)
              .NotEmpty()
              .WithMessage("Invalid Email ");

        }
    }
}
