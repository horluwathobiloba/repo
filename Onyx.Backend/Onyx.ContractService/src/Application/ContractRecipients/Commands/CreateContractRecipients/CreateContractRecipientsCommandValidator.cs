using FluentValidation; 

namespace Onyx.ContractService.Application.ContractRecipients.Commands.CreateContractRecipients
{
    public class CreateContractRecipientsCommandValidator: AbstractValidator<CreateContractRecipientsCommand>
    {
        public CreateContractRecipientsCommandValidator()
        { 
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
