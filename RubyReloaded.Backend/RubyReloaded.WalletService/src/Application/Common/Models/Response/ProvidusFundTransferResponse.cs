using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class ProvidusFundTransferResponse
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("transactionReference")]
        public string TransactionReference { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }

        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
    }
}
