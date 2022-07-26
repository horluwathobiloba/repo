using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Enums
{
    public enum Subscription
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
