using FluentValidation; 

namespace OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipients
{
    public class CreateRecipientsCommandValidator: AbstractValidator<CreateRecipientsCommand>
    {
        public CreateRecipientsCommandValidator()
        { 
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentId).GreaterThan(0).WithMessage("A valid document  must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
