using FluentValidation;


namespace OnyxDoc.DocumentService.Application.Commands.CreateDocumentSigningLink
{
    public class CreateDocumentSigningLinkCommandValidator : AbstractValidator<CreateDocumentSigningLinkCommand>
    {
        public CreateDocumentSigningLinkCommandValidator()
        { 
             RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.SigningAppUrl).NotEmpty().WithMessage("Signing App Url must be specified!");
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email must be specified!");
        }
    }
}
