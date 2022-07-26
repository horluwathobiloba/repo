using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using System; 
using OnyxDoc.SubscriptionService.Application.Payments.Commands;
using OnyxDoc.SubscriptionService.Application.Common.Models; 
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace OnyxDoc.SubscriptionService.API.Controllers.PayStack
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaystackController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor; 

        public PaystackController(IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor; 

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }       

        [HttpPost("createpaystackpayment")]
        public async Task<Result> CreatePaystackPayment(CreatePaystackPaymentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error initiating create Paystack payment request: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("verifypaystackpayment")]
        public async Task<Result> VerifyPaystackPayment(UpdatePaystackPaymentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error initiating verify Paystack payment request: {ex?.Message ?? ex?.InnerException?.Message }");
            }

        } 
    }
}
