using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
//using RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.CreateWalletBeneficiary;
using RubyReloaded.WalletService.Infrastructure.Utility;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.WithdrawalSettings.Commands;
using RubyReloaded.WalletService.Application.WithdrawalSettings.Queries.GetWithdrawalSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WithdrawalSettingsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WithdrawalSettingsController (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }


        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(AddWithdrawalSettingCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure( "Withdrawal Settings was not successful" + ex?.Message ?? ex?.InnerException?.Message );
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateWithdrawalSettingsCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Updating Withdrawal Settings was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetAllWithdrawalSettingsQuery());
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Withdrawal Settings retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyid/{userId}")]
        public async Task<ActionResult<Result>> GetById(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWithdrawalSettingsQuery { UserId = userId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Withdrawal Settings retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
