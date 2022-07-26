using Newtonsoft.Json;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.ViewModels
{

    public class PaystackAuthorizationRequest
    {
        public string AuthorizationCode { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

}
