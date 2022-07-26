using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class ProductItemCategory : AuditableEntity
    {
        public string DefaultImageUrl { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
