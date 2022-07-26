namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class NipFundTransferResponse
    {
        public string transactionReference { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }
}
