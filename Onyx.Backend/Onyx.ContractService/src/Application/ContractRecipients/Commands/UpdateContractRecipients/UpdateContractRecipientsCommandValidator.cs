using FluentValidation;
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipients
{
    public class UpdateContractRecipientsCommandValidator : AbstractValidator<UpdateContractRecipientsCommand>
    {
        public UpdateContractRecipientsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
