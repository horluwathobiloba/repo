using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class ProvidusValidationResponse
    {
        public string balance { get; set; }
        public string customer_name { get; set; }
        public string message { get; set; }
        public bool successful { get; set; }

    }
}
