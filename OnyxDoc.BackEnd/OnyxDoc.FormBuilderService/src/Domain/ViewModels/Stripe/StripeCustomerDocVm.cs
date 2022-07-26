using System; 
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
    public class StripeCustomerDocVm
    {

        //
        // Summary:
        //     Set this to true to create a file link for the newly created file. Creating a
        //     link is only possible when the file's purpose is one of the following: business_icon,
        //     business_logo, customer_signature, dispute_evidence, pci_document, or tax_document_user_upload.
        [JsonPropertyName("create")]
        public bool? Create { get; set; }
        //
        // Summary:
        //     A future timestamp after which the link will no longer be usable.
        //[JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt { get; set; }
 
        [JsonIgnore]
        [JsonPropertyName("deleted")] public bool? Deleted { get; set; }

        [JsonIgnore] 
        public bool? IdNumberProvided { get; set; }

        [JsonIgnore]
        public DateTime? CurrentDeadline { get; set; }

        [JsonIgnore]
        public string CurrentlyDue { get; set; }

        [JsonIgnore]
        public string PendingVerification { get; set; }

        [JsonIgnore]
        public string EventuallyDue { get; set; }

        [JsonIgnore]
        public string PastDue { get; set; }

        [JsonIgnore]
        public string CardPayments { get; set; }

        [JsonIgnore]
        public string Transfers { get; set; }

    }
}



