using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Enums
{
    public enum SubscriptionType
    {
        Active, 
        Inactive, 
        Basic, 
        Standard, 
        Premuim
    }
    public enum Revenue
    {
        TotalRevenue,
        Monthly,
        Weekly
        
    }
}
