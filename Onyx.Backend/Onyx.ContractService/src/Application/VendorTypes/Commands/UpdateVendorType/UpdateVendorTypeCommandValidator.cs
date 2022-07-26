using FluentValidation;
using Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorTypes
{
    public class UpdateVendorTypeCommandValidator : AbstractValidator<UpdateVendorTypeCommand>
    {
        public UpdateVendorTypeCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            //RuleFor(v => v.Id).NotEqual(0).WithMessage("Vendor type id must be specified!"); 
            //RuleFor(v => v.Name)
            //    .NotEmpty().WithMessage("Vendor type name must be specified!")
            //    .MaximumLength(200).WithMessage("Vendor type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


