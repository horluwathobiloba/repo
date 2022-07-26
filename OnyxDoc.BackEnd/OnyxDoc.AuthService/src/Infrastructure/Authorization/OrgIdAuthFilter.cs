using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Authorization
{
    [AttributeUsage(validOn:AttributeTargets.Class|AttributeTargets.Method)]
    public class OrgIdAuthFilter : Attribute, IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var identity = context.HttpContext.RequestServices.GetRequiredService<IIdentityService>();
           var accessToken = context.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
            var userid = accessToken.Claims.First(claim => claim.Type == "userid")?.Value;
            var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "SubscriberId").Value);
            var result = await identity.GetUserByIdAndSubscriber(userid, orgId);
            if (result.user==null)
            {
                context.Result = new UnauthorizedResult();
            }
            await next();
                
        }
    }
}
