using System;
using System.Collections.Generic;
using System.Text;
using RubyReloaded.WalletService.Domain.Common;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Bank : AuditableEntity
    {
        public string ShortName { get; set; }
        public int Code { get; set; }
        public bool IsPayoutBank { get; set; }
        public bool IsVirtualBank { get; set; }
        public string Currency { get; set; }
    }
}

