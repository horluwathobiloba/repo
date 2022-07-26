using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.Common.Models
{
    public class RefreshToken
    {
        public long ExpiresIn { get; set; }

        public string RefreshAccessToken { get; set; }
    }
}
