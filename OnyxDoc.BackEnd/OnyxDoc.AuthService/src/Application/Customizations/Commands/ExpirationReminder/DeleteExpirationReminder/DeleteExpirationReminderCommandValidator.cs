using FluentValidation;

namespace OnyxDoc.AuthService.Application.Customizations.Commands.ExpirationReminder.DeleteExpirationReminder
{
    public class DeleteExpirationReminderCommandValidator : AbstractValidator<DeleteExpirationReminderCommand>
    {
        public DeleteExpirationReminderCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Expiration reminder id must be specified");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id must be specified");
            RuleFor(x => x.SystemSettingsId).GreaterThan(0).WithMessage("System settings id must be specified");
            RuleFor(x => x.SubscriberId).GreaterThan(0).WithMessage("SubscriberId must be specified");
        }
    }
}
