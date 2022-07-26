using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Infrastructure.Authorization
{
    [AttributeUsage(validOn:AttributeTargets.Class|AttributeTargets.Method)]
    public class OrgIdAuthFilter : Attribute, IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var identity = context.HttpContext.RequestServices.GetRequiredService<IIdentityService>();
           var accessToken = context.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
            var userid = accessToken.Claims.First(claim => claim.Type == "userid").Value;
            var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
            var result = await identity.GetUserByIdAndOrganization(userid, orgId);
            if (result.staff==null)
            {
                context.Result = new UnauthorizedResult();
            }
            await next();
                
        }
    }
}
