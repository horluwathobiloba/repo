using FluentValidation;
using Onyx.WorkFlowService.Application.Staffs.Commands.UpdateStaff;

namespace Onyx.WorkFlowService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class UpdateRolePermissionsCommandValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.AccessLevel)
             .NotEmpty()
             .NotNull();

            RuleFor(v => v.RoleId)
                .NotEmpty();
        }
    }
}
