using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IStringHashingService
    {
        string CreateMD5StringHash(string input);

    }
}
