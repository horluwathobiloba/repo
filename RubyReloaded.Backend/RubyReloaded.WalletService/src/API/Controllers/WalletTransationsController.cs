using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Application.Common.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.CreateVirtualAccountConfig;
using RubyReloaded.WalletService.Application.Wallets.Queries;
using RubyReloaded.WalletService.Application.WalletTransactions.Queries;

namespace API.Controllers
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletTransationsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WalletTransationsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateWalletTransationCommand command)
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
                return Result.Failure(new string[] { "Wallet transaction creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletTransactionsQuery());
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet Transaction retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyuserid/{userId}")]
        public async Task<ActionResult<Result>> GetByUserId(string userId)
        {
            try
            {
               accessToken.ValidateToken(userId);
               
                return await Mediator.Send(new GetWalletTransactioneByUserIdQuery { UserId = userId});
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet Transaction retrieval by user id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getwallettransactionsbysearch/{search}")]
        public async Task<ActionResult<Result>> GetWalletTransactionBySearch(string search)
        {
            try
            {
              //  accessToken.ValidateToken(userId);

                return await Mediator.Send(new GetWalletTransactionBySearchValue {SearchValue=search });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet Transaction retrieval by user id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbywalletid/{userId}/{walletid}")]
        public async Task<ActionResult<Result>> GetByWalletId(string userId, int walletid)
        {
            try
            {
               accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletTransactioneByWalletIdQuery { UserId = userId, WalletId = walletid });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet Transaction retrieval by wallet id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getbytransactionid/{userId}/{transactionid}")]
        public async Task<ActionResult<Result>> GetByTransactionId(string userId, int transactionid)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletTransactionByTransactionIdQuery { TransactionId=transactionid});
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet Transaction retrieval by wallet id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
