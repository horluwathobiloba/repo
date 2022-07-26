using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Payments.Commands; 
using OnyxDoc.SubscriptionService.Infrastructure.Utility;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace OnyxDoc.SubscriptionService.API.Controllers.Stripe
{


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StripeController : ApiController
    { 

        protected readonly IHttpContextAccessor _httpContextAccessor;
        public StripeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("createcardpayment")]
        public async Task<ActionResult<Result>> CreateCardPayment(CreateStripeCardPaymentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Stripe Checkout was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("getpaymentintentstatus")]
        public async Task<ActionResult<Result>> GetPaymentStatus(UpdateStripeCardPaymentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Stripe payment was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        //[HttpPost("createstripeaccount")]
        //public async Task<ActionResult<Result>> CreateStripeAccount(StripeAccountVm command)
        //{
        //    try
        //    {
        //        command.AccessToken = accessToken;
        //        // return await Mediator.Send(command);
        //        var resp = await new StripeService(_configuration, _authService).CreateExternalBankAccount(command);
        //        return resp;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result.Failure(new string[] { "Stripe payment was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}
    }
}
