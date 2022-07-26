using FluentValidation;
using Onyx.AuthService.Application.Organizations.Commands.SignUpOrganization;

namespace Onyx.AuthService.Application.Organizations.Commands.SignUpLoanApplication
{
    public class SignUpOrganizationCommandValidator : AbstractValidator<SignUpOrganizationCommand>
    {
        public SignUpOrganizationCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty()
                .WithMessage("Invalid Organization name ");

            RuleFor(v => v.RCNumber)
              .MaximumLength(200)
              .NotEmpty()
              .WithMessage("Invalid RC Number");

        }
    }
}
