using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.WalletService.Application.Account.Queries.GetAccount;
using System.IO;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Application.Webhooks.Commands.SettlementNotificationCommand;

namespace API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WebhooksController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WebhooksController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized");
            }
        }
        [HttpPost]
        public async Task<ActionResult<ProvidusWebhookResponse>> SettlementNotification()
        {
            try
            {
                var notificationResponse = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                if (string.IsNullOrEmpty(notificationResponse))
                {
                    return BadRequest("Settlement Request is empty");
                }
                //get payment response status
                var settlementCommand = await Mediator.Send(new SettlementNotificationCommand { JsonResponse = notificationResponse });
                return settlementCommand;
            }
           
            catch (System.Exception ex)
            {
                throw  ex;
            }
        }

    }
}
