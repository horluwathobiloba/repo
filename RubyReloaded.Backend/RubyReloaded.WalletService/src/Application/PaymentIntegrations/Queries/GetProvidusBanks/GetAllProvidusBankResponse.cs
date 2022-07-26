using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks
{
    public class GetAllProvidusBankResponse
    {
        public List<Bank> banks { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
    }
    public class Bank
    {
        public string bankCode { get; set; }
        public string bankName { get; set; }
    }
}
