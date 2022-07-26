using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.VendorTypes.Queries.GetVendorTypes;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System; 

namespace Onyx.ContractService.Application.ProductServiceTypes.Queries.GetProductServiceTypes
{ 
    public class ProductServiceTypeDto : IMapFrom<ProductServiceType>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public int VendorTypeId { get; set; }
        public VendorTypeDto VendorType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap <ProductServiceType, ProductServiceTypeDto>();
            profile.CreateMap<ProductServiceTypeDto, ProductServiceType>();
            profile.CreateMap<VendorType, VendorTypeDto>();
            profile.CreateMap<VendorTypeDto, VendorType>();
        }
    }
}
