using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class ProductFundingSourceRequest
    {
        public ProductFundingCategory ProductFundingCategory { get; set; }
        public int PaymentChannelId { get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
        public int ProductId { get; set; }
    }
}
