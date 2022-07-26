using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class ProductSettlementAccountRequest
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public int BankId { get; set; }
    }
}
