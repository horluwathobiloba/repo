using Microsoft.AspNetCore.Http;
using OnyxDoc.FormBuilderService.Application.Common.Interfaces;
using System.Security.Claims;

namespace OnyxDoc.FormBuilderService.API.Services
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
