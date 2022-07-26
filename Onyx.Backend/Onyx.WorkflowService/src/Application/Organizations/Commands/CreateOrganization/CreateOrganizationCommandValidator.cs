using FluentValidation;
using Onyx.WorkFlowService.Application.Organizations.Commands.CreateOrganization;

namespace Onyx.WorkFlowService.Application.Organizations.Commands.CreateLoanApplication
{
    public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
    {
        public CreateOrganizationCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
