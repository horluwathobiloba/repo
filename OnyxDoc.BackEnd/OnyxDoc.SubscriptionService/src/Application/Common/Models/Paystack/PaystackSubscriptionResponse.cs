using Newtonsoft.Json;
using Paystack.Net;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{

    public class PaystackSubscriptionResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public SubscriptionData Data { get; set; }
    }


    public class PaystackSubscriptionListResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public List<SubscriptionData> Data { get; set; }
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }


    public class PaystackSubscriptionLinkResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public SubscriptionLinkData Data { get; set; }     
    }

    public class SubscriptionLinkData
    {
        [JsonProperty("link")]
        public string link { get; set; }
    }

    public class SubscriptionData
    {
        [JsonProperty("link")]
        public string link { get; set; }

        [JsonProperty("invoices")]
        public object[] invoices { get; set; }

        [JsonProperty("customer")]
        public Customer customer { get; set; }

        [JsonProperty("plan")]
        public PaystackPlan plan { get; set; }

        [JsonProperty("integration")]
        public int integration { get; set; }

        [JsonProperty("authorization")]
        public Authorization authorization { get; set; }

        [JsonProperty("domain")]
        public string domain { get; set; }

        [JsonProperty("start")]
        public int start { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("quantity")]
        public int quantity { get; set; }

        [JsonProperty("amount")]
        public int amount { get; set; }

        [JsonProperty("subscription_code")]
        public string subscription_code { get; set; }

        [JsonProperty("email_token")]
        public string email_token { get; set; }

        [JsonProperty("easy_cron_id")]
        public object easy_cron_id { get; set; }

        [JsonProperty("cron_expression")]
        public string cron_expression { get; set; }

        [JsonProperty("next_payment_date")]
        public DateTime next_payment_date { get; set; }

        [JsonProperty("open_invoice")]
        public object open_invoice { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime updatedAt { get; set; }
    }

    public class Customer
    {
        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phone")]
        public object phone { get; set; }

        [JsonProperty("metadata")]
        public Metadata metadata { get; set; }

        [JsonProperty("domain")]
        public string domain { get; set; }

        [JsonProperty("customer_code")]
        public string customer_code { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("integration")]
        public int integration { get; set; }

        [JsonProperty("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime updatedAt { get; set; }
    }

    public class PaystackPlan
    {
        [JsonProperty("domain")]
        public string domain { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("plan_code")]
        public string plan_code { get; set; }

        [JsonProperty("description")]
        public object description { get; set; }

        [JsonProperty("amount")]
        public int amount { get; set; }

        [JsonProperty("interval")]
        public string interval { get; set; }

        [JsonProperty("send_invoices")]
        public bool send_invoices { get; set; }

        [JsonProperty("send_sms")]
        public bool send_sms { get; set; }

        [JsonProperty("hosted_page")]
        public bool hosted_page { get; set; }

        [JsonProperty("hosted_page_url")]
        public object hosted_page_url { get; set; }

        [JsonProperty("hosted_page_summary")]
        public object hosted_page_summary { get; set; }

        [JsonProperty("currency")]
        public string currency { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("integration")]
        public int integration { get; set; }

        [JsonProperty("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime updatedAt { get; set; }
    }


}
