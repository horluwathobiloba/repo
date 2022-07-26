﻿namespace RubyReloaded.WalletService.Domain.ViewModels
{

    public class FlutterwaveResponse
    {
        public string status { get; set; }

        public string message { get; set; }

        public Data data { get; set; }

        public CustomerResponse customerresponse { get; set; }
    }
    public class Data
    {
        public int id { get; set; }
        public string link { get; set; }
        public string tx_ref { get; set; }
        public string flw_ref { get; set; }
        public string device_fingerprint { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string charged_amount { get; set; }
        public string app_fee { get; set; }
        public string merchant_fee { get; set; }
        public string processor_response { get; set; }
        public string auth_model { get; set; }
        public string ip { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string payment_type { get; set; }
        public string created_at { get; set; }
        public int account_id { get; set; }
        public decimal amount_settled { get; set; }
    }

    public class Card
    {
        public string first_6digits { get; set; }
        public string last_4digits { get; set; }
        public string issuer { get; set; }
        public string country { get; set; }
        public string type { get; set; }
        public string token { get; set; }
        public string expiry { get; set; }
    }

    public class CustomerResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string created_at { get; set; }
    }
}
