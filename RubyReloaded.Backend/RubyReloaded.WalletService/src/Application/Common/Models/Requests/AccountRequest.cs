using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class AccountRequest
    {
        public AccountType AccountType { get; set; }
        public int ParentAccountId { get; set; }
        public string CurrencyCode { get; set; }
        public AccountClass AccountClass { get; set; }
        public int ProductId { get; set; }
    }
}
