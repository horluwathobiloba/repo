using FluentValidation;
using Onyx.AuthService.Application.Organizations.Commands.UpdateOrganization;

namespace Onyx.AuthService.Application.Organizations.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
    {
        public UpdateOrganizationCommandValidator()
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
