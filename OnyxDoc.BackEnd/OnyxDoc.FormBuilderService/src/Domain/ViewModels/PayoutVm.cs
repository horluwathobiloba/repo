using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Domain.ViewModels
{
   public class PayoutVm
    {
        public long Amount { get; set; } 
        public string CurrencyCode { get; set; }
        public string OrderNo { get; set; }

        [JsonIgnore]
        public string PayoutId { get; set; }

        [JsonIgnore]
        public string Object { get; set; }

        [JsonIgnore]
        public DateTime ArrivalDate { get; set; }

        [JsonIgnore]
        public bool Automatic { get; set; }

        [JsonIgnore]
        public string BalanceTransaction { get; set; }

        [JsonIgnore]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        [JsonIgnore]
        public string Destination { get; set; }

        [JsonIgnore]
        public string FailureBalanceTransaction { get; set; }

        [JsonIgnore]
        public string FailureCode { get; set; }
       
        [JsonIgnore]
        public string FailureMessage { get; set; }

        [JsonIgnore]
        public bool LiveMode { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Metadata { get; set; }
        
        [JsonIgnore]
        public string Method { get; set; }

        [JsonIgnore]
        public string OriginalPayout { get; set; }

        [JsonIgnore]
        public string ReversedBy { get; set; }

        [JsonIgnore]
        public string SourceType { get; set; }

        [JsonIgnore]
        public string StatementDescriptor { get; set; }

        [JsonIgnore]
        public string Status { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public string PayoutDescription => $"Payout @:{DateTime.Now}";
    }
}
