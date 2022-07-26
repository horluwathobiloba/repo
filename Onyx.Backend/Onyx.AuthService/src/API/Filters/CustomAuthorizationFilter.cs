using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Filters
{
    public class CustomAuthorizationFilter
    {
        public class CustomAuthorizeAttribute : AuthorizeAttribute
        {
            private readonly string[] allowedAcessLevels;
            public CustomAuthorizeAttribute(params string[] policies)
            {
                this.allowedAcessLevels = policies;
            }

        }
    }
}
