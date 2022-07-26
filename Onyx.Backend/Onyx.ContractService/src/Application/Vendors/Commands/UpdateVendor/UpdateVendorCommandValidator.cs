using FluentValidation;
using Onyx.ContractService.Application.Vendors.Commands.UpdateVendor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Vendors.Commands.UpdateVendor
{
    public class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
    {
        public UpdateVendorCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).NotEqual(0).WithMessage("Vendor id must be specified!");
            RuleFor(v => v.VendorCompanyName)
               .NotEmpty().WithMessage("Vendor name must be specified!")
               .MaximumLength(200).WithMessage("Vendor name cannot exceed 200 characters length!");

            //RuleFor(v => v.ContactName).NotEmpty().WithMessage("Contact name must be specified!");
            //RuleFor(v => v.ContactEmail).NotEmpty().WithMessage("Contact email must be specified!");
            //RuleFor(v => v.ContactPhoneNumber).NotEmpty().WithMessage("Contact phone number must be specified!");

            //RuleFor(v => v.SupplierCode).NotEmpty().WithMessage("Supplier Code must be specified!");

            RuleFor(v => v.Address).NotEmpty().WithMessage("Address must be specified!");
            //RuleFor(v => v.Country).NotEmpty().WithMessage("Country must be specified!");
            //RuleFor(v => v.State).NotEmpty().WithMessage("State must be specified!");
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email must be specified!");
            RuleFor(v => v.PhoneNumber).NotEmpty().WithMessage("Phone number must be specified!");
            //RuleFor(v => v.SupplierClass).NotEmpty().WithMessage("Supplier class must be specified!");
            // RuleFor(v => v.ShortName).NotEmpty().WithMessage("Short Name must be specified!");
            RuleFor(v => v.VendorTypeId).GreaterThan(0).WithMessage("Vendor type Id must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


