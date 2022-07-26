namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class DynamicAccountNumberResponseDto
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public bool requestSuccessful { get; set; }
        public string responseCode { get; set; }
    }
}
