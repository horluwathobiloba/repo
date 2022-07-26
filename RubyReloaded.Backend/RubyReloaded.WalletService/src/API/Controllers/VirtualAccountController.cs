using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.VirtualAccount.Commands.CreateVirtualAccount;
using RubyReloaded.WalletService.Infrastructure.Utility;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.VirtualAccount.Query.GetVirtualAccount;
using RubyReloaded.WalletService.Application.VirtualAccount.Commands.ChangeVirtualAccount;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VirtualAccountController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public VirtualAccountController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateVirtualAccountCommand command)
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

                return Result.Failure(new string[] { "Virtual Account creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> EditVirtualAccount(ChangeVirtualAccountStatusCommand command)
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
                return Result.Failure(new string[] { "Updating Virtual Account Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetVirtualAccountQuery { UserId = userId });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Virtual Account retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetByVirtualAccountId(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetVirtualAccountByIdQuery { UserId = userId, VirtualAccountId = id });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Virtual Account retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
