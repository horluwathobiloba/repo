using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class TransactionServiceRequest
    {
        public TransactionServiceType TransactionServiceType { get; set; }
    }
}
