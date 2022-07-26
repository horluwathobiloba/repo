using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;

namespace Onyx.ContractService.Application.Common.Models
{
    public class UpdateProductServiceTypeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VendorTypeId { get; set; }
        public string VendorTypeName { get; set; } 
    }
}