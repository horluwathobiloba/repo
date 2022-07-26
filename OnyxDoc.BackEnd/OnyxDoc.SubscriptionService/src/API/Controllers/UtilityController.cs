using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models; 
using OnyxDoc.SubscriptionService.Application.Common.Exceptions; 
using OnyxDoc.SubscriptionService.Application.Utilities.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System.Linq;

namespace OnyxDoc.SubscriptionService.API.Controllers
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
                return await Task.Run(() => Result.Success(
                 ((CurrencyCode[])Enum.GetValues(typeof(CurrencyCode))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get currency enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("gettransactionratetypes")]
        public async Task<ActionResult<Result>> GetTransactionRateTypes()
        {
            try
            { 
                return await Task.Run(() => Result.Success(
                  ((TransactionRateType[])Enum.GetValues(typeof(TransactionRateType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get transaction rate type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptiontypes")]
        public async Task<ActionResult<Result>> GetSubscriptionTypes()
        {
            try
            { 
                return await Task.Run(() => Result.Success(
                  ((SubscriptionType[])Enum.GetValues(typeof(SubscriptionType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionfrequencies")]
        public async Task<ActionResult<Result>> GetSubscriptionFrequencies()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((SubscriptionFrequency[])Enum.GetValues(typeof(SubscriptionFrequency))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription frequency enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionstatus")]
        public async Task<ActionResult<Result>> GetSubscriptionStatus()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((SubscriptionStatus[])Enum.GetValues(typeof(SubscriptionStatus))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getpaymentstatus")]
        public async Task<ActionResult<Result>> GetPaymentStatus()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((PaymentStatus[])Enum.GetValues(typeof(PaymentStatus))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get payment status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getstoragesizetypes")]
        public async Task<ActionResult<Result>> GetStorageSizeTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((StorageSizeType[])Enum.GetValues(typeof(StorageSizeType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get storage size type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscribertypes")]
        public async Task<ActionResult<Result>> GetSubscriberTypes()
        {
            try
            {
                return await Task.Run(() => Result.Success(
                  ((StorageSizeType[])Enum.GetValues(typeof(StorageSizeType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                  ));
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscriber type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}