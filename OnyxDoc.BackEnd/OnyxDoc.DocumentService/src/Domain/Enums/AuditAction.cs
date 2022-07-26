using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Domain.Enums
{
    public enum AuditAction
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        LogIn = 4,
        LogOut = 5,
    }
}
