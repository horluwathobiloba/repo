using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Currency : AuditableEntity
    {
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeString { get; set; }
    }
}
