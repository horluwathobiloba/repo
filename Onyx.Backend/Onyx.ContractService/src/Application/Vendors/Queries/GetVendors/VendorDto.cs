using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Vendors.Queries.GetVendors
{
    public class VendorDto: IMapFrom<Domain.Entities.Vendor>
    {
        public int Id { get; set; }

        public string Name { get; set; } 
        public int VendorTypeId { get; set; }
        public VendorType VendorType { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string SupplierClass { get; set; }
        public string RCNumber { get; set; } 
        public string Address { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }

        public bool TouchPointVendor { get; set; } 

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Vendor, VendorDto>();
            profile.CreateMap<VendorDto, Domain.Entities.Vendor>();
        }
    }
}
