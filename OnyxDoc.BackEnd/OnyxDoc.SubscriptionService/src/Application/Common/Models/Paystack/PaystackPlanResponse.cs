using Newtonsoft.Json;
using Paystack.Net;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{

    public class PaystackPlanResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public PlanData Data { get; set; }
    }


    public class PaystackPlanListResponse
    {
        [JsonProperty("name")]
        public bool Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public List<PlanData> Data { get; set; }
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }


    public class PlanData
    {
        [JsonProperty("subscriptions")]
        public PaystackSubscription[] subscriptions { get; set; }

        [JsonProperty("integration")]
        public int integration { get; set; }

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

        [JsonProperty("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime updatedAt { get; set; }
    }

    public class PaystackSubscription
    {

        [JsonProperty("customer")]
        public int customer { get; set; }

        [JsonProperty("plan")]
        public int plan { get; set; }

        [JsonProperty("integration")]
        public int integration { get; set; }

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

        [JsonProperty("authorization")]
        public Authorization authorization { get; set; }

        [JsonProperty("easy_cron_id")]
        public string easy_cron_id { get; set; }

        [JsonProperty("cron_expression")]
        public string cron_expression { get; set; }

        [JsonProperty("next_payment_date")]
        public DateTime next_payment_date { get; set; }

        [JsonProperty("open_invoice")]
        public bool open_invoice { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime updatedAt { get; set; }
    }

    public class Authorization
    {
        [JsonProperty("authorization_code")]
        public string authorization_code { get; set; }

        [JsonProperty("bin")]
        public string bin { get; set; }

        [JsonProperty("last4")]
        public string last4 { get; set; }

        [JsonProperty("exp_month")]
        public string exp_month { get; set; }

        [JsonProperty("exp_year")]
        public string exp_year { get; set; }

        [JsonProperty("channel")]
        public string channel { get; set; }

        [JsonProperty("card_type")]
        public string card_type { get; set; }

        [JsonProperty("bank")]
        public string bank { get; set; }

        [JsonProperty("country_code")]
        public string country_code { get; set; }

        [JsonProperty("brand")]
        public string brand { get; set; }

        [JsonProperty("reusable")]
        public bool reusable { get; set; }

        [JsonProperty("signature")]
        public string signature { get; set; }

        [JsonProperty("account_name")]
        public string account_name { get; set; }
    }


    

}
