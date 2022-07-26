using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    
    public class GetProvidusAccountRequest
    {
      
        public string accountNumber { get; set; }
      
        public string userName { get; set; }
       
        public string password { get; set; }
    }
}
