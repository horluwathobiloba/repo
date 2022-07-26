using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class GetProvidusAccountResponse
    {
        public string accountStatus { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public string accountName { get; set; }
        public string bvn { get; set; }
        public string accountNumber { get; set; }
        public string cbaCustomerID { get; set; }
        public string responseMessage { get; set; }
        public string availableBalance { get; set; }
        public string responseCode { get; set; }
    }
}
