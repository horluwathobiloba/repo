using FluentValidation;  

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipientStatus
{
    public class UpdateRecipientStatusCommandValidator : AbstractValidator<UpdateRecipientStatusCommand>
    {
        public UpdateRecipientStatusCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
