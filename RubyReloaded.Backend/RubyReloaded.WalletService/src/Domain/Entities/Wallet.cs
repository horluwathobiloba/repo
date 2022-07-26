using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Wallet: AuditableEntity
    {
        public string WalletAccountNumber { get; set; }
        public string VirtualAccountNumber { get; set; }
        public string VirtualAccountName { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal Balance { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
