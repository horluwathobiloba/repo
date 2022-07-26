using FluentValidation;  

namespace Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipientStatus
{
    public class UpdateContractRecipientStatusCommandValidator : AbstractValidator<UpdateContractRecipientStatusCommand>
    {
        public UpdateContractRecipientStatusCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Contract type Initiator id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
