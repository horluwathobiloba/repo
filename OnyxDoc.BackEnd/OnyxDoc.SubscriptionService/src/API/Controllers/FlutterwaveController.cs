using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions; 
using OnyxDoc.SubscriptionService.Application.Payments.Commands;

namespace OnyxDoc.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FlutterwaveController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public FlutterwaveController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("initializeflutterwavepayment")]
        public async Task<ActionResult<Result>> InitializeFlutterwavePayment(InitializeFlutterwavePaymentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Initiliaze Flutterwave Payment failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("verifyflutterwavepayment")]
        public async Task<ActionResult<Result>> VerifyFlutterwavePayment(VerifyFlutterwavePaymentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Verify Flutterwave Payment failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

       
    }
}