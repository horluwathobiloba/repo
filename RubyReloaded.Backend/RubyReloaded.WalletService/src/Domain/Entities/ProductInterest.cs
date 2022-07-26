using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class ProductInterest : AuditableEntity
    {
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public decimal Rate { get; set; }
        public InterestType InterestType { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public ProductInterestInterval ProductInterestInterval { get; set; }
        //public ICollection<ProductInterestPeriod> ProductInterestPeriod { get; set; }
        public VariableType VariableType { get; set; }
    }
}
