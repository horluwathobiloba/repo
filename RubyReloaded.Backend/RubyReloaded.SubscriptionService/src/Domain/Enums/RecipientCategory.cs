using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.SubscriptionService.Domain.Enums
{
    public enum RecipientCategory
    {
        Owner = 1,
        Reviewer = 3,
        Approver = 2,
        InternalSignatory = 4,
        ExternalSignatory = 5
    }
}
