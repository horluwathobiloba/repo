using Onyx.ContractService.Domain.Common;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.ContractService.Domain.Entities
{
    public class Contract : AuditableEntity
    {
        public int InitialContractId { get; set; }
        public int RenewedContractId { get; set; }
        public int ContractTypeId { get; set; }
        public int PermitTypeId { get; set; }
        public ContractType ContractType { get; set; }
        public PermitType PermitType { get; set; }
        public int RoleId { get; set; }
        public int JobFunctionId { get; set; }
        public string JobFunctionName { get; set; }
        public string RoleName { get; set; }
        public int VendorId { get; set; }
        public int? VendorTypeId { get; set; }
        public int ProductServiceTypeId { get; set; }
        public int? PaymentPlanId { get; set; }
        public bool IsComplianceDueDiligence { get; set; }
        public bool IsTouchPointVendor { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeDesc { get; set; }
        public bool IsAnExecutedDocument { get; set; }
        public DurationFrequency DurationFrequency { get; set; }
        public string Currency{ get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ContractValue { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public string ContractStatusDesc { get; set; }
        public string TerminationReason { get; set; }
        public string RejectionReason { get; set; }
        public string UserId { get; set; }

        public string ExecutedContract { get; set; }
        public string ExecutedContractMimeType { get; set; }
        public string ExecutedContractFileExtension { get; set; } 

        public string InitiatorSignature { get; set; }
        public string InitiatorSignatureMimeType { get; set; }
        public string InitiatorSignatureFileExtension { get; set; } 
         
        //this is
        public string NextActorEmail { get; set; }
        public int NextActorRank { get; set; }
        public string NextActorAction { get; set; }

        #region Vendor Details
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SupplierClass { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string SupplierCode { get; set; }
        public string ShortName { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        //[DefaultValue(false)]
        public bool TouchPointVendor { get; set; }
        public string Address { get; set; }
        public string RCNumber { get; set; }
        #endregion

        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; }

        [ForeignKey(nameof(ProductServiceTypeId))]
        public ProductServiceType ProductServiceType { get; set; }

        [ForeignKey(nameof(PaymentPlanId))]
        public PaymentPlan PaymentPlan { get; set; }
        public int? ContractDurationId { get; set; }
        public ContractDuration ContractDuration { get; set; }
        public ICollection<ContractRecipient> ContractRecipients { get; set; } = new List<ContractRecipient>();
        public int? ContractTagId { get; set; }
        [ForeignKey(nameof(ContractTagId))]
        public ContractTag ContractTag { get; set; }
    }
}
