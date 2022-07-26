using System;
using System.Collections.Generic; 
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class StripeAccountVm
    {
        public string CustomerId { get; set; }

        /// <summary>
        /// REQUIRED: The account number for the bank account, in string form. Must be a checking account.
        /// </summary> 
        [JsonPropertyName("account_number")] public string AccountNumber { get; set; }

        /// <summary>
        /// REQUIRED: The country in which the bank account is located.
        /// </summary>
        [JsonPropertyName("country")] public string Country { get; set; }

        /// <summary>
        /// REQUIRED: The currency the bank account is in. This must be a country/currency pairing that Stripe supports.
        /// </summary>
        [JsonPropertyName("currency")] public string Currency { get; set; }

        [JsonPropertyName("default_for_currency")] public bool DefaultForCurrency { get; set; }

        public string DefaultCurrency { get; set; }

        /// <summary>
        /// OPTIONAL: The name of the person or business that owns the bank account.This field is required when attaching the bank account to a Customer object.
        /// </summary>
        [JsonPropertyName("account_holder_name")] public string AccountHolderName { get; set; }

        /// <summary>
        /// OPTIONAL: The type of entity that holds the account. This can be either individual or company.This field is required when attaching the bank account to a Customer object.
        /// </summary> 
        [JsonPropertyName("account_holder_type")] public string AccountHolderType { get; set; }

        /// <summary>
        /// OPTIONAL: The type of entity that holds the account. This can be either individual or company.This field is required when attaching the bank account to a Customer object.
        /// </summary> 
        [JsonPropertyName("bank_name")] public string BankName { get; set; }

        /// <summary>
        /// REQUIRED: The routing number, sort code, or other country-appropriate institution number for the bank account.For US bank accounts, this is required and should be the ACH routing number, not the wire routing number.If you are providing an IBAN for account_number, this field is not required.
        /// </summary>
        [JsonPropertyName("routing_number")] public string RoutingNumber { get; set; }

        [JsonIgnore]
        [JsonPropertyName("id")]
        public string AccountId { get; set; }

        [JsonIgnore]
        [JsonPropertyName("object")] public string Object { get; set; }

        [JsonIgnore]
        public bool LiveMode { get; set; }

        [JsonIgnore]
        [JsonPropertyName("metadata")] public Dictionary<string, string> Metadata { get; set; }

        [JsonIgnore]
        [JsonPropertyName("status")] public string Status { get; set; }

        [JsonIgnore]
        [JsonPropertyName("last4")] public string Last4 { get; set; }

        [JsonIgnore]
        [JsonPropertyName("fingerprint")] public string FingerPrint { get; set; }

        [JsonIgnore]
        [JsonPropertyName("account")] public string AccountNo { get; set; }

        [JsonIgnore]
        public DateTime Created { get; set; }
    }
}
