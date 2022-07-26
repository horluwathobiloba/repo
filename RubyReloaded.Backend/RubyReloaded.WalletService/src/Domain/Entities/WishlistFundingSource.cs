using System;
using System.Collections.Generic;
using RubyReloaded.WalletService.Domain.Enums;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class WishlistFundingSource
    {
        public int Id { get; set; }
        public string FundingSource { get; set; }
        public FundingSourceCategory FundingSourceCategory { get; set; }
        public int PaymentChannelId { get; set; }
    }
}
