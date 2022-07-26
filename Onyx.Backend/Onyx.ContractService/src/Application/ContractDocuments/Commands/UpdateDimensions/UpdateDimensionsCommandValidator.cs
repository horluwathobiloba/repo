using FluentValidation;
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDocuments.Commands.UpdateDimensions
{
    public class UpdateDimensionsCommandValidator : AbstractValidator<UpdateDimensionsCommand>
    {
        public UpdateDimensionsCommandValidator()
        {
            RuleFor(v => v.OrganizationId).GreaterThan(0).WithMessage("Organization must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract must be specified!");
           
        }
    }
}
