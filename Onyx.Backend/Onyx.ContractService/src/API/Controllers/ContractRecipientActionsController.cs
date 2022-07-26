using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Application.ContractRecipientActions.Commands.ApproveAction;
using Microsoft.AspNetCore.Http;
using System;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractRecipientActionsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractRecipientActionsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("approvecontract")]
        public async Task<ActionResult<Result>> ApproveContract(ApproveContractActionCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Contract approval failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


        [HttpPost("rejectcontract")]
        public async Task<ActionResult<Result>> RejectContract(RejectContractActionCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Contract rejection failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("signcontract")]
        public async Task<ActionResult<Result>> SignContract(SignContractActionCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Contract signing failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

    }
}
