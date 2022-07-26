using FluentValidation;

namespace Onyx.WorkFlowService.Application.Roles.Commands.ChangeRole
{
    public class ChangeRoleStatusCommandValidator : AbstractValidator<ChangeRoleStatusCommand>
    {
        public ChangeRoleStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.RoleId)
              .NotEmpty();


        }
    }
}
