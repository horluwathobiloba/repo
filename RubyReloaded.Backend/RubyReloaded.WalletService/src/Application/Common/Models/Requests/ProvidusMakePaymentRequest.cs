using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class ProvidusMakePaymentRequest
    {
        public string billId { get; set; }
        public string customerAccountNo { get; set; }
        public string amount { get; set; }
        //public string transaction_ref { get; set; }
        public string channel_ref { get; set; }
        //public Input[] inputs { get; set; }
        public List<Input> Inputs { get; set; }
    }

    public class Input
    {
        public string key { get; set; }
        public string value { get; set; }
    }


}
