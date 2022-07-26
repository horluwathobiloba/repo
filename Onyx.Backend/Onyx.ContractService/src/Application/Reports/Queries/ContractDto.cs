using AutoMapper;
using Onyx.ContractService.Application.Common.Mappings;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;
using Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes;
using Onyx.ContractService.Application.PaymentPlans.Queries.GetPaymentPlans;
using Onyx.ContractService.Application.ProductServiceTypes.Queries.GetProductServiceTypes;
using Onyx.ContractService.Application.SupportingDocuments.Queries.GetSupportingDocuments;
using Onyx.ContractService.Application.Vendors.Queries.GetVendors;
using Onyx.ContractService.Domain.Entities;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Reports.Queries.GetReports
{
    public class ContractDto : IMapFrom<Domain.Entities.Contract>
    {
        public int Id { get; set; }
        public int ContractTypeId { get; set; }
        public int PermitTypeId { get; set; }
        public int OrganisationId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int VendorId { get; set; }
        public int ProductServiceTypeId { get; set; }
        public int PaymentPlanId { get; set; }
        public bool IsComplianceDueDiligence { get; set; }
        public bool IsTouchPointVendor { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
        public int? ContractDurationId { get; set; }
        public decimal ContractValue { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public string ContractStatusDesc { get; set; }
        public string UserId { get; set; }
        //contract files 
        public string InitiatorSignature { get; set; }
        public string InitiatorSignatureFileExtension { get; set; }
        public string InitiatorSignatureMimeType { get; set; }

        public string ExecutedContract { get; set; }
        public string ExecutedContractFileExtension { get; set; }
        public string ExecutedContractMimeType { get; set; }

        public string NextActorEmail { get; set; }
        public int NextActorRank { get; set; }
        public string NextActorAction { get; set; }

        #region Vendor Details
        public string Email { get; set; }
        public string SupplierClass { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        #endregion

        public VendorDto Vendor { get; set; }
        public ProductServiceTypeDto ProductServiceType { get; set; }
        //public PaymentPlanDto PaymentPlan { get; set; }
        public string CreatedBy { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public List<SupportingDocumentDto> SupportingDocuments{ get; set; }
        //public List<ContractRecipientDto> ContractRecipients { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Contract, ContractDto>();
            profile.CreateMap<ContractDto, Domain.Entities.Contract>();

            //profile.CreateMap<Vendor, VendorDto>();
            //profile.CreateMap<VendorDto, Vendor>();

            //profile.CreateMap<ProductServiceType, ProductServiceTypeDto>();
            //profile.CreateMap<ProductServiceTypeDto, ProductServiceType>();

            //profile.CreateMap<PaymentPlan, PaymentPlanDto>();
            //profile.CreateMap<PaymentPlanDto, PaymentPlan>();

            ////profile.CreateMap<ContractRecipient, ContractRecipientDto>();
            ////profile.CreateMap<ContractRecipientDto, ContractRecipient>();

            //profile.CreateMap<SupportingDocument, SupportingDocumentDto>();
            //profile.CreateMap<SupportingDocumentDto, SupportingDocument>();
        }
    }
}
