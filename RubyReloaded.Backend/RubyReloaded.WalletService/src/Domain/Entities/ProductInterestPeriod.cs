using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System.Collections.Generic;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class ProductInterestPeriod : AuditableEntity
    {
        public int ProductInterestId { get; set; }
        public ProductInterest ProductInterest { get; set; }
        public ProductInterestInterval HoldingPeriod { get; set; }
    }
}
