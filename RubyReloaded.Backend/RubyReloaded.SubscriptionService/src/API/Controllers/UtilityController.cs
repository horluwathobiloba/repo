using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.SubscriptionService.Application.Common.Models; 
using RubyReloaded.SubscriptionService.Application.Common.Exceptions; 
using RubyReloaded.SubscriptionService.Application.Utilities.Queries;

namespace RubyReloaded.SubscriptionService.API.Controllers
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UtilityController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public UtilityController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            //accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            //if (accessToken == null)
            //{
            //    throw new Exception("You are not authorized!");
            //}
        }

        [HttpGet("getcurrencies")]
        public async Task<ActionResult<Result>> GetCurrencies()
        {
            try
            {
                return await Mediator.Send(new GetCurrencyEnums() );
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving currency enums was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("gettransactionratetypes")]
        public async Task<ActionResult<Result>> GetTransactionRateTypes()
        {
            try
            {
                return await Mediator.Send(new GetTransactionRateTypeEnums());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving transaction rate type enums was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptiontypes")]
        public async Task<ActionResult<Result>> GetSubscriptionTypes()
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionTypeEnums() );
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving subscription type enums was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionfrequencies")]
        public async Task<ActionResult<Result>> GetSubscriptionFrequencies()
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionFrequencyEnums() );
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving subscription frequency enums was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}