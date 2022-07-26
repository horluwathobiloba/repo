using FluentValidation;
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipient
{
    public class UpdateContractRecipientCommandValidator : AbstractValidator<UpdateContractRecipientCommand>
    {
        public UpdateContractRecipientCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Contract recipient must be specified!");
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract must be specified!");
            RuleFor(v => v.Email).NotEmpty().WithMessage("A valid email must be specified!");
            RuleFor(v => v.RecipientCategory).IsInEnum().WithMessage("A valid recipient category must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
