using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.CreateBankBeneficiaries;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.UpdateBankBeneficiaries;
using RubyReloaded.WalletService.Application.BankBeneficiaries.Queries.GetBankBeneficiaries;
using RubyReloaded.WalletService.Application.BankBeneficiaries.Commands.DeleteBankBeneficiary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BankBeneficiaryController:ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BankBeneficiaryController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateBankBeneficiaryCommand command)
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
        public async Task<ActionResult<Result>> UpdateBankBeneficiary(UpdateBankBeneficiariesCommand command)
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
                return Result.Failure(new string[] { "Updating Beneficiaries was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetBankBeneficiariesQuery { UserId = userId });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Beneficiaries retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyid/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetById(string userId, int id)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetBankBeneficiaryById { UserId = userId, BeneficiaryId = id });
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
        [HttpPost("delete/{id}/{userid}")]
        public async Task<ActionResult<Result>> Delete(int id, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new DeleteBankBeneficiaryCommand { Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure( "Beneficiary retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message);
            }
        }

    }
}
