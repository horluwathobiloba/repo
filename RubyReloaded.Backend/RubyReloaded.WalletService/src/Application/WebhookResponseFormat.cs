using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class WebhookResponseFormat
    {

        public bool RequestSuccessful { get; set; }
        public string SessionId { get; set; }
        public string ResponseMessage { get; set; }
        public string responseCode { get; set; }
    }
}
