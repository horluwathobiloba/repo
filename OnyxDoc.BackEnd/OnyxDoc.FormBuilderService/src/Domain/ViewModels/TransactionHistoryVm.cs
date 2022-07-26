using System;
using System.Collections.Generic;
using System.Text;

namespace Anelloh.APIServices.Domain.ViewModels
{
    public class TransactionHistoryVm
    {
        public string Name { get; set; }
        public string OrderNo { get; set; }
        public decimal ToSwap { get; set; }
        public decimal For { get; set; }
        public decimal Rate { get; set; }
        public decimal TransactionFee { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
