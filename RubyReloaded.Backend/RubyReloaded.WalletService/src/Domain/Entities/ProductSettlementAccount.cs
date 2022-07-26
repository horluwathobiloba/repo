using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class ProductSettlementAccount : AuditableEntity
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public int BankId { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

    }
}
