using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Contracts.Commands.CreateContract;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.Contracts.Commands.UpdateStatus;
using Onyx.ContractService.Application.Contracts.Commands.UpdateContractStatus;
using Onyx.ContractService.Application.Contracts.Commands.UpdateExecutedContract;
using Onyx.ContractService.Application.Contracts.Queries.GetExecutedContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Application.ContractRecipientActions.Commands.ApproveAction;
using Onyx.ContractService.Application.Contracts.Commands.UpdateContract;
using Onyx.ContractService.Application.Contracts.Commands.ContractEmailNotification;
using Onyx.ContractService.Application.Contracts.Queries;
using Onyx.ContractService.Application.DashBoards;
using Microsoft.AspNetCore.Http;
using System;
using Onyx.ContractService.Application.Reports.Queries.GetReports;
using Onyx.ContractService.Application.Reports.Queries;
using Onyx.ContractService.Application.Reports.Command;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ReportsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }


        [HttpPost("contractreports")]
        public async Task<ActionResult<Result>> ContractAndPermitReport(ContractReportCommand command)
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

                return Result.Failure($"Contract Reports failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpPost("permitreports")]
        public async Task<ActionResult<Result>> PermitReport(PermitReportCommand command)
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

                return Result.Failure($"Permit Reports failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }




    }
}