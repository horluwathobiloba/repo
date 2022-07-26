using Onyx.ContractService.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class ProductServiceType : AuditableEntity
    { 
        public int VendorTypeId { get; set; } 

        [ForeignKey(nameof(VendorTypeId))]
        public virtual VendorType VendorType { get; set; }
    }
}
