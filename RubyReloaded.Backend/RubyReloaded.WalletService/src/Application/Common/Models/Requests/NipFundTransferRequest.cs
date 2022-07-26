namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class NipFundTransferRequest
    {
        public string beneficiaryAccountName { get; set; }
        public string transactionAmount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string sourceAccountName { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryBank { get; set; }
        public string transactionReference { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
