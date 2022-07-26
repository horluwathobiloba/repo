using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class ProductInterestRequest
    {
        public int MinimumAmount { get; set; }
        public int MaximumAmount { get; set; }
        public int Rate { get; set; }
        public InterestType InterestType { get; set; }
        public ICollection<ProductInterestPeriodRequest> ProductInterestPeriod { get; set; }
        public VariableType VariableType { get; set; }
    }
}
