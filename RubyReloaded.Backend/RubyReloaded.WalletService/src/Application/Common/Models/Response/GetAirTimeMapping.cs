using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class GetAirtimeMapping
    {
        public int bill_id { get; set; }
        public double fee { get; set; }
        public int field_id { get; set; }
        public List<FieldAirtime> fields { get; set; }
        public string type { get; set; }
        public string validate { get; set; }
    }

    public class ListAirtime
    {
        public List<object> items { get; set; }
        public string list_type { get; set; }
    }

    public class FieldAirtime
    {
        public string call_tag { get; set; }
        public string field_type { get; set; }
        public string key { get; set; }
        public ListAirtime list { get; set; }
        public string name { get; set; }
        public string narration_order { get; set; }
    }

}
