using MediatR;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.CreateCurrencyConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.CurrencyConfigurations.Commands.ChangeCurrencyConfigurations;
using RubyReloaded.AuthService.Application.CurrencyConfigurations.Queries.GetCurrencyConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;

namespace API.Controllers
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CurrencyConfigurationController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CurrencyConfigurationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("createcurrencyconfiguration")]
        public async Task<ActionResult<Result>> CreateCurrencyConfiguration(CreateCurrencyConfigurationCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency configuration creation was not successful. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpPost("changecurrencyconfigurationstatus")]
        public async Task<ActionResult<Result>> ChangeCurrencyStatus(ChangeCurrencyConfigurationStatusCommand command)
        {
            //validation
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Currency configuration update was not successful. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
        [HttpGet("getallcurrenciesenums/{userid}")]
        public async Task<ActionResult<Result>> GetAllCurrenciesEnum(string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetCurrencyEnums());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all Currencies was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAllCurrencies(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetCurrencyConfigurationsQuery { Skip=skip,Take=take});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all Currencies was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivecurrencies/{userid}")]
        public async Task<ActionResult<Result>> GetActiveCurrencies(string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetActiveCurrencyConfigurationsQuery());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");

            }
        }
    }
}
