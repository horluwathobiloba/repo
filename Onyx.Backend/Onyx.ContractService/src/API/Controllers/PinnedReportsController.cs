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
using Onyx.ContractService.Application.Contractaudit.Commands.CreatePinnedReportsCommand;
using Onyx.ContractService.Application.ContractDuration.Queries.GetPinnedReport;
using Onyx.ContractService.Application.PinnedReports.Commands.CreatePinnedReportsCommand;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PinnedReportsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PinnedReportsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }


        [HttpPost("createpinnedreports")]
        public async Task<ActionResult<Result>> CreatePinnedReport(CreatePinnedReportsCommand command)
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

                return Result.Failure($"Create pinned reports failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpPost("updatepinnedreports")]
        public async Task<ActionResult<Result>> UpdatePinnedReport(UpdatePinnedReportsCommand command)
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

                return Result.Failure($"Update pinned reports failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpPost("terminatepinnedreports")]
        public async Task<ActionResult<Result>> TerminatePinnedReport(TerminatePinnedReportCommand command)
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

                return Result.Failure($"Terminate pinned reports failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getpinnedreportsbyorganizationid/{organizationid}")]
        public async Task<ActionResult<Result>> GetPinnedReportsByOrganizationId(int organizationid)
        {
            try
            {
                return await Mediator.Send(new GetPinnedReportsByOrganisationIdQuery { OrganisationId = organizationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"An error occured while retrieving Audits. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getpinnedreportsbyorganizationidandid/{organizationid}/{id}")]
        public async Task<ActionResult<Result>> GetPinnedReportsByOrganizationIdAndId(int organizationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetPinnedReportsByOrganisationIdAndIdQuery { OrganisationId = organizationid, Id=id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"An error occured while retrieving Audits. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }




    }
}