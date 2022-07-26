using FluentValidation;
using Onyx.WorkFlowService.Application.Roles.Commands.CreateRole;

namespace Onyx.WorkFlowService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommandValidator : AbstractValidator<CreateRolePermissionsCommand>
    {
        public CreateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty()
              .NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty()
                .NotNull();

            //RuleFor(v => v.RoleId)
            //    .NotEmpty();

            //RuleFor(v => v.AccessLevel)
            //    .NotEmpty()
            //    .NotNull();
            
        }
    }
}
