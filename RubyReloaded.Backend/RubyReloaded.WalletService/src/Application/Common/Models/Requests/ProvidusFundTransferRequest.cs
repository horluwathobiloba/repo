using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class ProvidusFundTransferRequest
    {
        //    [JsonProperty("creditAccount")]
        //    public string CreditAccount { get; set; }
        //    [JsonProperty("debitAccount")]
        //    public string DebitAccount { get; set; }
        //    [JsonProperty("transactionAmount")]
        //    public string TransactionAmount { get; set; }
        //    [JsonProperty("currencyCode")]
        //    public string CurrencyCode { get; set; }
        //    [JsonProperty("narration")]
        //    public string Narration { get; set; }
        //    [JsonProperty("transactionReference")]
        //    public string TransactionReference { get; set; }
        //    [JsonProperty("userName")]
        //    public string UserName { get; set; }
        //    [JsonProperty("password")]
        //    public string Password { get; set; }

        
            public string creditAccount { get; set; }
            public string debitAccount { get; set; }
            public string transactionAmount { get; set; }
            public string currencyCode { get; set; }
            public string narration { get; set; }
            public string transactionReference { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
        

    }
}
