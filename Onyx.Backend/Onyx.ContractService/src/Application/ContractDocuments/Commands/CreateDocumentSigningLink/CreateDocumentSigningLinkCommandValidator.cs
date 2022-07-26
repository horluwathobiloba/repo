using FluentValidation; 

namespace Onyx.ContractService.Application.Contract.Commands.CreateDocumentSigningLink
{
    public class CreateDocumentSigningLinkCommandValidator : AbstractValidator<CreateDocumentSigningLinkCommand>
    {
        public CreateDocumentSigningLinkCommandValidator()
        { 
             RuleFor(v => v.OrganizationId).NotEqual(0).WithMessage("Organisation Id must be specified!");
            RuleFor(v => v.ContractId).NotEqual(0).WithMessage("Contract Id must be specified!");
            RuleFor(v => v.SigningAppUrl).NotEmpty().WithMessage("Signing App Url must be specified!");
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email must be specified!");
        }
    }
}
