using FluentValidation;
using Onyx.ContractService.Application.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Onyx.ContractService.Application.Vendors.Commands.CreateVendors
{
    public class CreateVendorsCommandValidator : AbstractValidator<CreateVendorsCommand>
    {
        public CreateVendorsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Vendors).NotNull().WithMessage("No vendor record found.");
            RuleFor(v => v.Vendors).Must(ValidateVendors).WithMessage("Vendor records have invalid or empty data.");
        }

        private bool ValidateVendors(ICollection<CreateVendorRequest> list)
        {
            if (list.Count <= 0)
            {
                throw new System.Exception("No vendor record found.");
            }
            var badRequests = (from x in list
                               where x.VendorTypeId == 0 || string.IsNullOrEmpty(x.VendorCompanyName)  
                              || string.IsNullOrEmpty(x.Address)  || string.IsNullOrEmpty(x.Email) || string.IsNullOrEmpty(x.PhoneNumber)
                               select x).ToList();
            if (badRequests.Count > 0)
            {
                throw new System.Exception($"{badRequests.Count} Vendor records have invalid or empty data.");
            }
            return true;
        }
    }

}
