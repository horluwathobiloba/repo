using FluentValidation;
using Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractDuration.Commands.UpdateContractDurations
{
    public class UpdateContractDurationsValidator : AbstractValidator<UpdateContractDurations>
    {
        public UpdateContractDurationsValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


