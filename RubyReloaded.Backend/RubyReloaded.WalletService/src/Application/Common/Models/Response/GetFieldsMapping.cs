using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Response
{
    public class GetFieldsMapping
    {
        public int bill_id { get; set; }
        public double fee { get; set; }
        public int field_id { get; set; }
        public List<Field> fields { get; set; }
        public string type { get; set; }
        public string validate { get; set; }
    }
    public class List
    {
        public List<object> items { get; set; }
        public string list_type { get; set; }
    }

    public class Field
    {
        public string call_tag { get; set; }
        public string field_type { get; set; }
        public string key { get; set; }
        public List list { get; set; }
        public string name { get; set; }
        public string narration_order { get; set; }
    }
}
