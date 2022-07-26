using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Upskillz_invoice_mgt_Application.Common.Interfaces;

namespace Upskillz_invoice_mgt_api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}
