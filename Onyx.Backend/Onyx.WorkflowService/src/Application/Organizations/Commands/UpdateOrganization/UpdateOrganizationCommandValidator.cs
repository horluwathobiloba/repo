using FluentValidation;
using Onyx.WorkFlowService.Application.Organizations.Commands.UpdateOrganization;

namespace Onyx.WorkFlowService.Application.Organizations.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
    {
        public UpdateOrganizationCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
