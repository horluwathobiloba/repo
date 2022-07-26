using FluentValidation;
using Onyx.ContractService.Application.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Onyx.ContractService.Application.Vendors.Commands.UpdateVendors
{
    public class UpdateVendorsCommandValidator : AbstractValidator<UpdateVendorsCommand>
    {
        private string error;
        public UpdateVendorsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");

            RuleFor(v => v.Vendors).Must(ValidateVendors).WithMessage(error);
        }

        private bool ValidateVendors(ICollection<UpdateVendorRequest> list)
        {
            if (list.Count <= 0)
            {
                error = "No vendor record found for update.";
                return false;
            }
            var badRequests = (from x in list
                               where x.VendorTypeId == 0 || string.IsNullOrEmpty(x.VendorCompanyName) 
                              || string.IsNullOrEmpty(x.Address) || string.IsNullOrEmpty(x.Email)
                               || string.IsNullOrEmpty(x.PhoneNumber)
                               select x).ToList();
            if (badRequests.Count > 0)
            {
                error = $"{badRequests.Count} Vendor records have invalid or empty data.";
                return false;
            }
            return true;
        }
    }
}


