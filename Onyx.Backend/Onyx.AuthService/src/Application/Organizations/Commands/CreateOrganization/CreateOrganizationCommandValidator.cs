using FluentValidation;
using Onyx.AuthService.Application.Organizations.Commands.CreateOrganization;

namespace Onyx.AuthService.Application.Organizations.Commands.CreateLoanApplication
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationCommandValidator()
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
