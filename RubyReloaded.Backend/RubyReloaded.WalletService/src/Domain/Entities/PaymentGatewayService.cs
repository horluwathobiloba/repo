using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class PaymentGatewayService : AuditableEntity
    {
        public PaymentGatewayServiceCategory PaymentGatewayServiceCategory { get; set; }
         public int PaymentChannelId { get; set; }
        public PaymentChannel PaymentChannel { get; set; }
    }
}
