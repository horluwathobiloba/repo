using FluentValidation;
using Onyx.WorkFlowService.Application.Departments.Commands.CreateDepartment;
using Onyx.WorkFlowService.Application.Organizations.Commands.CreateOrganization;

namespace Onyx.WorkFlowService.Application.Organizations.Commands.CreateDepartment
{
    public class ChangeDepartmentStatusCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public ChangeDepartmentStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            //RuleFor(v => v.Names)
            //    .MaximumLength(200)
            //    .NotEmpty();
        }
    }
}
