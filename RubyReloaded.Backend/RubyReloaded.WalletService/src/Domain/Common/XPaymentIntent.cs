using System.Text.Json.Serialization;

namespace RubyReloaded.WalletService.Domain.Common
{
    public abstract class XPaymentIntent
    {
        public string AuthorizationCode { get; set; }
        public string TransactionReference { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TransactionFee { get; set; }
        public string CallBackUrl { get; set; }
        public string Email { get; set; }
        public string ClientReferenceId { get; set; }
        public string PaymentMethodType { get; set; }

        [JsonIgnore]
        public string SessionId { get; set; }

        [JsonIgnore]
        public string SuccessUrl { get; set; }

        [JsonIgnore]
        public string CancelUrl { get; set; }

        [JsonIgnore]
        public long Quantity => 1;

        [JsonIgnore]
        public string Mode => "payment";

        [JsonIgnore]
        public string Description => $"Payment for Wallet Funding";
    }
}