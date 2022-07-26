using Microsoft.AspNetCore.ApiAuthorization;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Filters
{
    public class CustomApiAuthorizationFilter
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
