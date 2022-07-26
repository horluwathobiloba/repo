namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class TokenVm
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TokenDto Token { get; set; }
    }
}
