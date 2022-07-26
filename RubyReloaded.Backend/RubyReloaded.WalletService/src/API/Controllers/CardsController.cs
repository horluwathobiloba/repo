using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Infrastructure.Utility;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks;
using RubyReloaded.WalletService.Application.Cards.Commands;
using RubyReloaded.WalletService.Application.Cards.Commands.DeactivateCardAuthorization;
using RubyReloaded.WalletService.Application.Cards.Commands.GetCardAuthorization;

namespace RubyReloaded.WalletService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

           accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("addcard")]
        public async Task<ActionResult<Result>> AddCard(AddCardAuthorizationCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Adding Card failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatecard")]
        public async Task<ActionResult<Result>> DeactivateCard(DeactivateCardAuthorizationCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Deactivate Card failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("chargecard")]
        public async Task<ActionResult<Result>> ChargeCard(ChargeCardAuthorizationCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Charge Card failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

      

        [HttpGet("getcards/{userId}")]
        public async Task<ActionResult<Result>> GetCards(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                accessToken.ValidateBVN();
                return await Mediator.Send(new GetCardAuthorizationByUserIdQuery { UserId = userId});
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Card Authorization was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


    }
}