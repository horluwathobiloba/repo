using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Enums
{
    public enum ContractStatus
    {
        Processing = 1,
        PendingApproval = 2,
        Approved = 3,
        PendingSignatories = 4,
        Active = 5,
        Rejected = 6,
        Expired = 7,
        Cancelled = 8,
        Terminated = 9,
        PendingActivation = 10
    }
}
