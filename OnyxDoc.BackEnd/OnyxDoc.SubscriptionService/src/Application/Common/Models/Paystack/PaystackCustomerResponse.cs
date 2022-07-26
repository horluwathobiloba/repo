using Newtonsoft.Json;
using Paystack.Net;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{


    public class PaystackCustomerResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public CustomerData Data { get; set; }
    }

    public class PaystackCustomerListResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public List<CustomerData> Data { get; set; }
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }


    public class CustomerData
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("integration")]
        public string Integration { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("customer_code")]
        public string CustomerCcode { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("identified")]
        public bool Identified { get; set; }

        [JsonProperty("identifications")]
        public List<Identification> Identifications { get; set; }

        [JsonProperty("metadata")]
        public Metadata MetaData { get; set; }
 
        [JsonProperty("transactions")]
        public object[] Transactions { get; set; }

        [JsonProperty("subscriptions")]
        public object[] Subscriptions { get; set; }

        [JsonProperty("authorizations")]
        public Dictionary<string, object> Authorizations { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class Data
    {
        public int integration { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public object phone { get; set; }
        public Dedicated_Account dedicated_account { get; set; }
        public bool identified { get; set; }
        public Identification[] identifications { get; set; }
       
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Dedicated_Account
    {
        [JsonProperty("bank")]
        public Bank Bank { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("account_name")]
        public string AccountName { get; set; }
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("split_config")]
        public Split_Config SplitConfig { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("assigned")]
        public bool Assigned { get; set; }
        [JsonProperty("assignment")]
        public Assignment Assignment { get; set; }
    }

    public class Bank
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public class Split_Config
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("integration")]
        public int Integration { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("split_code")]
        public string SplitCode { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("bearer_type")]
        public string BearerType { get; set; }
        [JsonProperty("bearer_subaccount")]
        public object BearerSubAccount { get; set; }
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("is_dynamic")]
        public bool IsDynamic { get; set; }
        [JsonProperty("subaccounts")]
        public Subaccount[] subaccounts { get; set; }
        [JsonProperty("total_subaccounts")]
        public int total_subaccounts { get; set; }
    }

    public class Subaccount
    {
        [JsonProperty("subaccount")]
        public Subaccount SubAccount { get; set; }

        [JsonProperty("share")]
        public int Share { get; set; }
    }

    public class SubAccount
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("subaccount_code")]
        public string SubaccountCode { get; set; }
        [JsonProperty("business_name")]
        public string BusinessName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("primary_contact_name")]
        public object PrimaryContactName { get; set; }
        [JsonProperty("primary_contact_email")]
        public object PrimaryContactEmail { get; set; }
        [JsonProperty("primary_contact_phone")]
        public object PrimaryContactPhone { get; set; }
        [JsonProperty("metadata")]
        public object Metadata { get; set; }
        [JsonProperty("settlement_bank")]
        public string SettlementBank { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
    }

    public class Assignment
    {
        [JsonProperty("assignee_id")]
        public int AssigneeIid { get; set; }
        [JsonProperty("assignee_type")]
        public string AssigneeType { get; set; }
        [JsonProperty("account_type")]
        public string AccountType { get; set; }
        [JsonProperty("integration")]
        public int Integration { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("photos")]
        public Photo[] photos { get; set; }
    }

    public class Photo
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("typeId")]
        public string TypeId { get; set; }
        [JsonProperty("typeName")]
        public string TypeName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("isPrimary")]
        public bool IsPrimary { get; set; }
    }

    public class Identification
    {
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Meta
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("skipped")]
        public int Skipped { get; set; }
        [JsonProperty("perPage")]
        public int PerPage { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }
       
    }     
  
}
