namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class APIRequestDto
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public object requestObject { get; set; }
        public bool IsFormData { get; set; }
        public string ClientId{ get; set; }
        public string XAuthSignature { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
