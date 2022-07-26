using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Enums
{
    public enum UserCreationStatus
    {
        Invited=1,
        Approved,
        Rejected,
        AccessRequest
    }
}
