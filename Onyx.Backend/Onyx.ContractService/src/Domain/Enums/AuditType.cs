using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Enums
{
    public enum  AuditType
    {
        Create = 1,
        Update = 2,
        Approve=3,
        Generate=4,
        Sign=5,
        Terminate =6,
        Renew = 7
    }
}
