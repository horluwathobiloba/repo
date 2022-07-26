using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OnyxDoc.FormBuilderService.Domain.Common
{
    public abstract class XPaymentIntent
    {
        public string SubscriptionNo { get; set; }
        public decimal UnitAmount { get; set; }

       
        public long Quantity { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TransactionFee { get; set; }
        public string CallBackUrl { get; set; }
        public string Email { get; set; }
        public string ClientReferenceNo { get; set; }
        public string PaymentMethodType { get; set; }

        [JsonIgnore]
        public string SessionId { get; set; }

        [JsonIgnore]
        public string SuccessUrl { get; set; }

        [JsonIgnore]
        public string CancelUrl { get; set; }

        [JsonIgnore]
        public long UnitAmountInt64
        {
            get
            {
                return Convert.ToInt64(this.UnitAmount);
            }
        }

        [JsonIgnore]
        public string Mode => "payment";

        [JsonIgnore]
        public string Description => $"Payment for subscription number: {this.SubscriptionNo}";
    }
}
