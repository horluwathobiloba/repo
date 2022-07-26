using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.DashBoard.Queries;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using RubyReloaded.WalletService.Infrastructure.Utility;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashBoardController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public DashBoardController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpGet("getdashboardrecenttransactions/{userid}")]
        public async Task<ActionResult<Result>> GetDashBoardRecentTransactions(string userid)
        {
            try
            {
                //accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(new GetDashBoardRecentTransactions { UserId = userid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting mapping was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getdashboardwalletactivities/{userid}")]
        public async Task<ActionResult<Result>> GetDashBoardWalletActivities(string userid)
        {
            try
            {
                //accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(new GetDashBoardWalletActivities { UserId=userid});
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting values was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getdashboardwalletbalancequery/{userid}")]
        public async Task<ActionResult<Result>> GetDashBoardWalletBalanceQuery(string userid)
        {
            try
            {
                //accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(new GetDashBoardWalletBalanceQuery { UserId = userid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting values was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getfrequentlytransferred/{userid}")]
        public async Task<ActionResult<Result>> GetFrequentlyTransferred(string userid)
        {
            try
            {
                //accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(new GetFrequentlyTransferredQuery { UserId = userid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Getting values was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
