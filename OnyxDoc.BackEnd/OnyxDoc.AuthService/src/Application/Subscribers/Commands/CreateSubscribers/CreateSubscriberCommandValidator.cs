using FluentValidation;
using OnyxDoc.AuthService.Application.Subscribers.Commands.CreateSubscriber;

namespace OnyxDoc.AuthService.Application.Subscribers.Commands.CreateSubscriber
{
    public class CreateSubscriberCommandValidator : AbstractValidator<CreateSubscriberCommand>
    {
        public CreateSubscriberCommandValidator()
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
