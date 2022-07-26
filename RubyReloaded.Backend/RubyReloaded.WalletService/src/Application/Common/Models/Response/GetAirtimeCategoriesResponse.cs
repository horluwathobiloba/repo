using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class GetAirtimeCategoriesResponse
    {
        public int bill_id { get; set; }
        public string category_id { get; set; }
        public string commission_id { get; set; }
        public string description { get; set; }
        public string fee_id { get; set; }
        public string list_order { get; set; }
        public string name { get; set; }
        public string provider_id { get; set; }
        public string settlement_account { get; set; }
        public string status { get; set; }
        public string source_id { get; set; }
    }


    //public class AirtimeCategory
    //{
    //    public int bill_id { get; set; }
    //    public string category_id { get; set; }
    //    public string commission_id { get; set; }
    //    public string description { get; set; }
    //    public string fee_id { get; set; }
    //    public string list_order { get; set; }
    //    public string name { get; set; }
    //    public string provider_id { get; set; }
    //    public string settlement_account { get; set; }
    //    public string status { get; set; }
    //}

    

}
