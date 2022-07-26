using FluentValidation;

namespace Onyx.WorkFlowService.Application.Staffs.Commands.UpdateStaff
{
    public class UpdateStaffCommandValidator : AbstractValidator<UpdateStaffCommand>
    {
        public UpdateStaffCommandValidator()
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

            //RuleFor(v => v.Email)
            //.MaximumLength(200)
            //.NotEmpty();


            RuleFor(v => v.PhoneNumber)
           .MaximumLength(14)
           .NotEmpty();
        }
    }
}
