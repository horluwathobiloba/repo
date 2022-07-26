using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Currencies.Commands;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.Currencies.Queries;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;

namespace OnyxDoc.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CurrencyController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CurrencyController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateCurrencyCommand command)
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
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcurrencies")]
        public async Task<ActionResult<Result>> CreateCurrencies(CreateCurrenciesCommand command)
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
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateCurrencyCommand command)
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
                return Result.Failure($"Currency update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }


        [HttpPost("updatecurrencies")]
        public async Task<ActionResult<Result>> UpdateCurrencies(UpdateCurrenciesCommand command)
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
                return Result.Failure($"Update currencies failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecurrencystatus")]
        public async Task<ActionResult<Result>> UpdateCurrencyStatus(UpdateCurrencyStatusCommand command)
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
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }


        [HttpGet("getcurrencybyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetCurrencyById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetCurrencyByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving currency failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getcurrencies/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetCurrencies(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetCurrenciesQuery { SubscriberId = subscriberid, UserId= userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all currencies was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getcurrenciesbycurrencycode/{subscriberid}/{currencycode}/{userid}")]
        public async Task<ActionResult<Result>> GetCurrenciesByCurrencyCode(int subscriberid, string currencycode, string userid)
        {
            try
            {
                return await Mediator.Send(new GetCurrenciesByCurrencyCodeQuery
                { SubscriberId = subscriberid, CurrencyCode = currencycode, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get currencies by currency code was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getactivecurrencies/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveCurrencies(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveCurrenciesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active currencies was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivecurrencynames/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveCurrencyNames(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveCurrencyNamesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active subscrption pricings was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }
    }
}