using FluentValidation;

namespace Onyx.AuthService.Application.Customers.Commands.VerifyEmail
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
