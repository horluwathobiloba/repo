using FluentValidation;

namespace Onyx.AuthService.Application.Organizations.Commands.ChangeOrganizationStatus
{
    public class ChangeOrganizationStatusCommandValidator : AbstractValidator<ChangeOrganizationStatusCommand>
    {
        public ChangeOrganizationStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

        
        }
    }
}
