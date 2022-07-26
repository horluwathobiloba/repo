using Microsoft.AspNetCore.Authorization;

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
