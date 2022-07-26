using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Domain.Enums
{
    public enum WorkflowLevelAction
    {
        ViewOnly = 1,
        Review = 2,
        Approve = 3,
        InternalSignature = 4,
        ExternalSignature = 5,
        ThirdPartySignature = 6,
        WitnessSignature = 7
    }
}
