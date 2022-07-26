using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.WalletService.Application.Account.Queries.GetAccount;
using RubyReloaded.WalletService.Application.Accounts.Commands.CreateCustomerAccount;
using RubyReloaded.WalletService.Application.Accounts.Commands;
using RubyReloaded.WalletService.Application.Accounts.Queries.GetAccount;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public AccountsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if(accessToken == null)
            {
                throw new Exception("You are not authorized");
            }
        }

        [HttpPost("createcustomeraccount")]
        public async Task<ActionResult<Result>> CreateCustomerAccount(CreateCustomerAccountCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.CustomerId);
              //  accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch(ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Customer Account creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("createglaccount")]
        public async Task<ActionResult<Result>> CreateGLAccount(CreateGLAccountCommand command)
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

                return Result.Failure(new string[] { "GL Account creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetAccountQuery { UserId = userId });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Accountss retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetByAccountId(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetAccountByIdQuery { UserId = userId, AccountId = id });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Accounts retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getaccountbyentity/{userid}/{search}")]
        public async Task<ActionResult<Result>> GetAccountByEntity(string userid, string search)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAccountByEntityQuery { SearchValue=search});
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Accounts retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getaccountsbyuserid/{userid}")]
        public async Task<ActionResult<Result>> GetAccountByUserId(string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAccountByUserIdQuery { UserId = userid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Accounts retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getaccountbyaccountnumber/{userid}")]
        public async Task<ActionResult<Result>> GetAccountByAccountNumber(string userid,string accountNumber)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAccountByAccountNumberQuery { AccountNumber = accountNumber });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Accounts retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
