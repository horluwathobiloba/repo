using RubyReloaded.WalletService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class WithdrawalSetting:AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
    }
}
