using FluentValidation;
using Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorTypeStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorTypeStatus
{
    class UpdateVendorStatusValidator : AbstractValidator<UpdateVendorTypeStatusCommand>
    {
        public UpdateVendorStatusValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Vendor id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
