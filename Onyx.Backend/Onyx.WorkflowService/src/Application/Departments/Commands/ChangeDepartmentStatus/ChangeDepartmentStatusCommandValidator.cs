using FluentValidation;

namespace Onyx.WorkFlowService.Application.Departments.Commands
{
    public class ChangeDepartmentStatusCommandValidator : AbstractValidator<ChangeDepartmentStatusCommand>
    {
        public ChangeDepartmentStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.DepartmentId)
              .NotEmpty();


        }
    }
}
