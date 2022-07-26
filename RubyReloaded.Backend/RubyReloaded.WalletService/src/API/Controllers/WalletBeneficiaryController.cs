using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.CreateWalletBeneficiary;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.UpdateWalletBeneficiary;
using RubyReloaded.WalletService.Application.WalletBeneficiaries.Queries.GetWalletBeneficiaries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RubyReloaded.WalletService.Application.WalletBeneficiaries.Commands.DeleteWalletBeneficiary;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletBeneficiaryController: ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WalletBeneficiaryController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateWalletBeneficiaryCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUser);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiary creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<Result>> UpdateWalletBeneficiary(UpdateWalletBeneficiaryCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUser);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Changing Service Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletBeneficiariesQuery { UserId = userId });
            }
            catch (System.Exception ex)
            {
               return Result.Failure(new string[] { "Service retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        

        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetById(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletBeneficiaryByIdQuery { UserId = userId, BeneficiaryId = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiary retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyuserid/{userId}")]
        public async Task<ActionResult<Result>> GetByUserId(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetWalletBeneficiariesByUserId { UserId = userId});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiary retrieval by userid was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("delete/{id}/{userid}")]
        public async Task<ActionResult<Result>> Delete(int id,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new DeleteWalletBeneficiaryCommand { Id=id});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiary retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
