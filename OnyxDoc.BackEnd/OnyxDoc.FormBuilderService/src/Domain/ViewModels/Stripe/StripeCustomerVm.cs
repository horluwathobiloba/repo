using System;
using System.Collections.Generic; 
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
    public class StripeConnectedAccountVm
    {
        /// <summary>
        /// REQUIRED: The type of Stripe account to create. May be one of custom, express or standard.
        /// </summary> 
        [JsonPropertyName("type")] public string Type { get; set; }
        //Summary:
        //      OPTIONAL: The country in which the account holder resides, or in which the business is legally established. 
        //      This should be an ISO 3166-1 alpha-2 country code. For example, if you are in the United States and the business 
        //      for which you’re creating an account is legally represented in Canada, you would use CA as the country for the account being created.
        [JsonPropertyName("country")] public string Country { get; set; }
        //
        //Summary:
        //      REQUIRED: The email address of the account holder. This is only to make the account easier to identify to you. 
        //      Stripe will never directly email Custom accounts.
        [JsonPropertyName("email")] public string Email { get; set; }
        [JsonPropertyName("phone")] public string Phone { get; set; }
        [JsonPropertyName("first_name")] public string FirstName { get; set; }
        [JsonPropertyName("last_name")] public string LastName { get; set; }
        [JsonPropertyName("maiden_name")] public string MaidenName { get; set; }
        //
        //Summary:
        //       The individual’s gender (International regulations require either “male” or “female”).
        [JsonPropertyName("gender")] public string Gender { get; set; }
        //
        // Summary:
        //      The government-issued ID number of the individual, as appropriate for the representative’s country. 
        //      (Examples are a Social Security Number in the U.S., or a Social Insurance  Number in Canada).
        //      Instead of the number itself, you can also provide a PII token created with Stripe.js.
        [JsonPropertyName("id_number")] public string IdNumber { get; set; }
        //
        // Summary:
        //     The last four digits of the individual's Social Security Number (U.S. only).
        [JsonPropertyName("ssn_last_4")] public bool? SsnLast4Provided { get; set; }
        //
        // Summary:
        //     The back of an ID returned by a file upload with a purpose value of identity_document.
        //     The uploaded file needs to be a color image (smaller than 8,000px by 8,000px),
        //     in JPG, PNG, or PDF format, and less than 10 MB in size.
        [JsonPropertyName("back")] public string Back { get; set; }
        //
        // Summary:
        //     The front of an ID returned by a file upload with a purpose value of identity_document.
        //     The uploaded file needs to be a color image (smaller than 8,000px by 8,000px),
        //     in JPG, PNG, or PDF format, and less than 10 MB in size.
        [JsonPropertyName("front")] public string Front { get; set; }
        //
        // Summary:
        //     Indicates if the person or any of their representatives, family members, or other
        //     closely related persons, declares that they hold or have held an important public
        //     job or function, in any jurisdiction. One of: existing, or none.
        [JsonPropertyName("political_exposure")] public string PoliticalExposure { get; set; }


        [JsonPropertyName("city")] public string AddressCity { get; set; }
        [JsonPropertyName("country")] public string AddressCountry { get; set; }
        [JsonPropertyName("line1")] public string AddressLine1 { get; set; }
        [JsonPropertyName("line2")] public string AddressLine2 { get; set; }
        [JsonPropertyName("postal_code")] public string AddressPostalCode { get; set; }
        [JsonPropertyName("state")] public string AddressState { get; set; }

        //
        // Summary:
        //     The business type. One of: company, government_entity, individual, or non_profit.
        [JsonPropertyName("business_type")]
        public string BusinessType { get; set; }
        [JsonIgnore]
        [JsonPropertyName("card_payments.requested")]
        public bool CardPaymentsRequested { get; set; }

        [JsonIgnore]
        [JsonPropertyName("transfers.requested")]
        public bool TransfersRequested { get; set; }

        [JsonIgnore]
        [JsonPropertyName("id")]
        public string CustomerId { get; set; }

        [JsonIgnore]
        [JsonPropertyName("object")] public string Object { get; set; }

        [JsonIgnore]
        public bool LiveMode { get; set; }

        [JsonIgnore]
        [JsonPropertyName("metadata")] public Dictionary<string, string> Metadata { get; set; }

        [JsonIgnore]
        [JsonPropertyName("status")] public string Status { get; set; }

        [JsonIgnore]
        [JsonPropertyName("charges_enabled")] public bool ChargesEnabled { get; set; }

        [JsonIgnore]
        [JsonPropertyName("payouts_enabled")] public bool PayoutsEnabled { get; set; }

        [JsonIgnore]
        [JsonPropertyName("fingerprint")] public string FingerPrint { get; set; }

        [JsonIgnore]
        [JsonPropertyName("default_currency")] public string DefaultCurrency { get; set; }

        [JsonIgnore]
        [JsonPropertyName("details_submitted")] public bool DetailsSubmitted { get; set; }

        [JsonIgnore]
        [JsonPropertyName("created")] public DateTime Created { get; set; }
        [JsonIgnore]
        [JsonPropertyName("disabled_reason")] public string DisabledReason { get; set; }

        [JsonIgnore]
        [JsonPropertyName("tos_acceptance.date")] public string TosAcceptanceDate { get; set; }

        /// <summary>
        /// The user’s service agreement type.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("tos_acceptance.service_agreement")] public string TosAcceptanceServiceAgreement { get; set; }
        [JsonIgnore]
        [JsonPropertyName("tos_acceptance.user_agent")] public string TosAcceptanceUserAgent { get; set; }
        [JsonIgnore]
        [JsonPropertyName("tos_acceptance.ip")] public string TosAcceptanceIp { get; set; }

        [JsonIgnore]
        [JsonPropertyName("avs_failure")] public string DeclineOnAvsFailure { get; set; }
        [JsonIgnore]
        [JsonPropertyName("cvc_failure")] public string DeclineOnCvcFailure { get; set; }

        [JsonIgnore]
        [JsonPropertyName("display_name")] public string DashboardDisplayName { get; set; }

        [JsonIgnore]
        [JsonPropertyName("timezone")] public string DashboardTimezone { get; set; }

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



