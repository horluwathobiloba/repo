using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Wallets.Commands;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.Wallets.Queries;
using Microsoft.AspNetCore.Http;
using RubyReloaded.WalletService.Infrastructure.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.WalletService.Application.Wallets.Commands.TransferWalletToWallet;

using RubyReloaded.WalletService.Application.Wallets.Commands.CommandTransfer;
using RubyReloaded.WalletService.Application.Wallets.Commands.ProvidusFundTransferCommand;
using RubyReloaded.WalletService.Domain.Enums;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletsController:ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WalletsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateWalletCommand command)
        {
            try
            {
               // accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Wallet creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("transfer")]
        public async Task<ActionResult<Result>> TransferFunds(TransferCommand command)
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

                return Result.Failure(new string[] { "Wallet creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("providustransfer")]
        public async Task<ActionResult<Result>> ProvidusTransfer(ProvidusFundTransferCommand command)
        {
            try
            {
                //accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Wallet creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("transfertowallet")]
        public async Task<ActionResult<Result>> TransferToWallet(TransferWalletToWalletCommand command)
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

                return Result.Failure(new string[] { "Wallet creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getwalletbalance/{userId}/{walletAccountNumber}")]
        public async Task<ActionResult<Result>> GetWalletBalance(string userId, string walletAccountNumber)
        {
            try
            {
               // accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletBalanceQuery { UserId = userId, WalletAccountNumber = walletAccountNumber });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet balance retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getwalletdetails/{userId}")]
        public async Task<ActionResult<Result>> GetWalletDetails(string userId)
        {
            try
            {
                // accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletsDetailsQuery { UserId = userId });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet details retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getwalletdetailsbyentity/{search}")]
        public async Task<ActionResult<Result>> GetWalletDetailsByEntity(string search)
        {
            try
            {
                // accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletByEntity { SearchValue = search });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Wallet details retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("gettransactionfee/{amount}/{bankservicecategory}")]
        public async Task<ActionResult<Result>> GetTransactionFee(decimal amount, BankServiceCategory bankservicecategory)
        {
            try
            {
                // accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetTransactionFeeQuery { BankServiceCategory = bankservicecategory,Amount=amount });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Transaction fee was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
