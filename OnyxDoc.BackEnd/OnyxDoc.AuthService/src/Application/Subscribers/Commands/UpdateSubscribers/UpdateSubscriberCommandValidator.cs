using FluentValidation;

namespace OnyxDoc.AuthService.Application.Subscribers.Commands.UpdateSubscriber
{
    public class UpdateSubscriberCommandValidator : AbstractValidator<UpdateSubscriberCommand>
    {
        public UpdateSubscriberCommandValidator()
        {
            RuleFor(v => v.Name)
                 .MaximumLength(200)
                 .NotEmpty();

            RuleFor(v => v.ContactEmail)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
