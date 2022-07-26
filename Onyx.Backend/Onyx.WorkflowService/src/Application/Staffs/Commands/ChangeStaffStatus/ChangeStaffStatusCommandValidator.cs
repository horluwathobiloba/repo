using FluentValidation;

namespace Onyx.WorkFlowService.Application.Staffs.Commands.ChangeStaffStatus
{
    public class ChangeStaffStatusCommandValidator : AbstractValidator<ChangeStaffStatusCommand>
    {
        public ChangeStaffStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.StaffId)
              .NotEmpty();


        }
    }
}
