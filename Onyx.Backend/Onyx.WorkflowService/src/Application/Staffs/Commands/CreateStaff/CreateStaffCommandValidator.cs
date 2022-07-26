using FluentValidation;
using Onyx.WorkFlowService.Application.Staffs.Commands.CreateStaff;

namespace Onyx.WorkFlowService.Application.Organizations.Commands.CreateStaff
{
    public class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
    {
        public CreateStaffCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
                .NotEmpty();

            RuleFor(v => v.RoleId)
                .NotEmpty();

            RuleFor(v => v.FirstName)
                 .MaximumLength(200)
                 .NotEmpty();

            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.Email)
            .MaximumLength(200)
            .NotEmpty();

            RuleFor(v => v.PhoneNumber)
           .MaximumLength(14)
           .NotEmpty();
        }
    }
}
