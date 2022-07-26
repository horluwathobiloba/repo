using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Models.Paystack
{
    public class PaystackPaymentResponseRaw
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
    public class Data
    {
        [JsonPropertyName("authorization_url")]
        public string Authorization_Url { get; set; }



        [JsonPropertyName("access_code")]
        public string Access_Code { get; set; }



        [JsonPropertyName("reference")]
        public string Reference { get; set; }



        [JsonPropertyName("domain")]
        public string Domain { get; set; }



        [JsonPropertyName("status")]
        public string Status { get; set; }



        [JsonPropertyName("gateway_response")]
        public string Gateway_Response { get; set; }



        [JsonPropertyName("card")]
        public string Card { get; set; }



        [JsonPropertyName("currency")]
        public string Currency { get; set; }



        [JsonPropertyName("fees")]
        public decimal? Fees { get; set; }



        [JsonPropertyName("feesplit")]
        public decimal? FeeSplit { get; set; }



        [JsonPropertyName("requested_amount")]
        public decimal? Requested_Amount { get; set; }
    }



    public class Authorization
    {
        [JsonPropertyName("authorization_code")]
        public string Authorization_Code { get; set; }



        [JsonPropertyName("bin")]
        public string Bin { get; set; }



        [JsonPropertyName("last4")]
        public string Last4 { get; set; }



        [JsonPropertyName("exp_month")]
        public string Exp_Month { get; set; }
        [JsonPropertyName("exp_year")]
        public string Exp_Year { get; set; }
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("card_type")]
        public string Card_Type { get; set; }
        [JsonPropertyName("bank")]
        public string Bank { get; set; }
        [JsonPropertyName("country_code")]
        public string Country_Code { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
        [JsonPropertyName("account_name")]
        public string Account_Name { get; set; }
    }
}