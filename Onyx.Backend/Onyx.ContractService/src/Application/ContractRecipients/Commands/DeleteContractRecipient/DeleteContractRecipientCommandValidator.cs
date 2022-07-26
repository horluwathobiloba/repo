using FluentValidation; 
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.DeleteContractRecipient
{
    public class DeleteContractRecipientCommandValidator : AbstractValidator<DeleteContractRecipientCommand>
    {
        public DeleteContractRecipientCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Contract recipient must be specified!"); 
        }
    }
}
