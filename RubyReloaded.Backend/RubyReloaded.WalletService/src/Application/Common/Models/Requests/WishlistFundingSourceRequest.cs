using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class WishlistFundingSourceRequest
    {
        public string FundingSource { get; set; }
        public FundingSourceCategory FundingSourceCategory { get; set; }
        public int PaymentChannelId { get; set; }
    }
}
