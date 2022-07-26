using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Enums
{
    public enum  AccessLevel
    {
        SuperAdmin, 
        Admin, 
        PowerUser,
        ExternalUser,
        Support
    }
}
