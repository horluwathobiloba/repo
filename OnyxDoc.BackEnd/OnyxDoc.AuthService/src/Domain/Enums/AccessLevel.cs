using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Enums
{
    public enum  AccessLevel
    {
        Individual=1,
        Corporate=2,
        SystemOwner=3,
        IndividualAndCorporate=4,
        CorporateAndSystemOwner=5,
        IndividualAndSystemOwner=6,
        All=7
    }
}