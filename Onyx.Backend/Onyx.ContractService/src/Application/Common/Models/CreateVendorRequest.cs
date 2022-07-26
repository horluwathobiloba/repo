using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Common.Models
{
    public class CreateVendorRequest
    {
        public string RCNumber { get; set; }
        public string VendorCompanyName { get; set; }
        public int VendorTypeId { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //contact details
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string SupplierClass { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public bool TouchPointVendor { get; set; }

    }
}
