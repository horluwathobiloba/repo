using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class TransactionService : AuditableEntity
    {
        public TransactionServiceType TransactionServiceType { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }

}
