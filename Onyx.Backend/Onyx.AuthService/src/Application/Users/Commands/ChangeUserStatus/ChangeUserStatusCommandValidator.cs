using FluentValidation;

namespace Onyx.AuthService.Application.Users.Commands.ChangeUserStatus
{
    public class ChangeUserStatusCommandValidator : AbstractValidator<ChangeUserStatusCommand>
    {
        public ChangeUserStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.UserId)
              .NotEmpty();


        }
    }
}
