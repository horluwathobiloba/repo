using System;
using System.Collections.Generic;
using System.Text;
using RubyReloaded.WalletService.Domain.Common;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class VirtualAccount : AuditableEntity
    {
        public int WalletId { get; set; }
        public int BankId { get; set; }
        public string AccountNo { get; set; } 
        public Wallet Wallet { get; set; }
        public Bank Bank { get; set; }
    }
}
