using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class Vendor : AuditableEntity
    {
        public string RCNumber { get; set; }
        public int VendorTypeId { get; set; } 
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }

        public string SupplierClass { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        [DefaultValue(false)]
        public bool TouchPointVendor { get; set; }
        [ForeignKey(nameof(VendorTypeId))]
        public VendorType VendorType { get; set; }
        public ICollection<Contract> Contracts { get; set; }

    }
}
