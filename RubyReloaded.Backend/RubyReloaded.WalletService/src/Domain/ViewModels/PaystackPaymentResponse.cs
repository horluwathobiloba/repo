using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.ViewModels
{

    public class PaystackPaymentResponse
    {
        public ResponseData data { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
    }

    public class ResponseData
    {

        public string access_code { get; set; }
        public string authorization_url { get; set; }
        public string reference { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string gateway_response { get; set; }
        public string card { get; set; }
        public string currency { get; set; }
        public decimal? fees { get; set; }
        public decimal? feesplit { get; set; }
        public Authorization authorization { get; set; }
        public decimal? requested_amount { get; set; }
    }

    public class Authorization
    {
        public string authorization_code { get; set; }
        public string bin { get; set; }
        public string last4 { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string channel { get; set; }
        public string card_type { get; set; }
        public string bank { get; set; }
        public string country_code { get; set; }
        public string brand { get; set; }
        public string signature { get; set; }
        public string account_name { get; set; }
    }
}
