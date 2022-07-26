using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class ProductFundingSource : AuditableEntity
    {
        public ProductFundingCategory ProductFundingCategory { get; set; }
        public int PaymentChannelId { get; set; }
        public Payment PaymentChannel{ get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
