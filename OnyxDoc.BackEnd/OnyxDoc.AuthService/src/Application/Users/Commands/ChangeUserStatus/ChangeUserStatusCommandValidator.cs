using FluentValidation;

namespace OnyxDoc.AuthService.Application.Users.Commands.ChangeUserStatus
{
    public class ChangeUserStatusCommandValidator : AbstractValidator<ChangeUserStatusCommand>
    {
        public ChangeUserStatusCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
              .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.UserId)
              .NotEmpty();


        }
    }
}
