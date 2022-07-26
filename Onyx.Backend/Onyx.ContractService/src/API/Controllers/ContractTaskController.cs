using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTask.Command;
using Onyx.ContractService.Application.ContractTask.Queries;
using Onyx.ContractService.Application.ContractTasks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractTaskController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractTaskController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("createcontracttask")]
        public async Task<ActionResult<Result>> Create(CreateContractTaskCommand command)
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
                return Result.Failure($"Contract creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createmultiplecontracttasks")]
        public async Task<ActionResult<Result>> CreateMultipleTasks(CreateContractTaskToMultipleUsersCommand command)
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
                return Result.Failure($"Creation of multiple contract task failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createresolvedcontracttasks")]
        public async Task<ActionResult<Result>> CreateResolvedTasks(CreateResolvedTask command)
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
                return Result.Failure($"Creation of resolved contract task failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontracttask")]
        public async Task<ActionResult<Result>> UpdateContractTask(UpdateContractTaskCommand command)
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
                return Result.Failure($"Update Contract task failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getunresolvedcontracttasksbyorganisationid/{organisationid}")]
        public async Task<ActionResult<Result>> GetUnresolvedContractTasks(int organisationId)
        {
            try
            {
                return await Mediator.Send(new GetUnresolvedContractTaskQuery {OrganisationId = organisationId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get unresolved task was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getresolvedcontracttasksbyorganisationid/{organisationid}")]
        public async Task<ActionResult<Result>> GetResolvedContractsTasks(int organisationId)
        {
            try
            {
                return await Mediator.Send(new GetResolvedContractTaskQuery {OrganisationId = organisationId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get resolved task was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpGet("getcontracttasksbyidandorganisationid/{id}/{organisationId}")]
        public async Task<ActionResult<Result>> GetContractsTasksById(int id, int organisationId)
        {
            try
            {
                return await Mediator.Send(new GetContractTaskByIdQuery { Id = id, OrganisationId=organisationId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get contract task by id was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttasksbycontract/{organisationid}/{contractid}")]
        public async Task<ActionResult<Result>> GetContractTagsByContract(int organisationid, int contractid)
        {
            try
            {
                return await Mediator.Send(new GetContractTasksByContractQuery { OrganisationId = organisationid, ContractId = contractid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract tasks retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttasksbyorganisationid/{organisationId}")]
        public async Task<ActionResult<Result>> GetContractsTasksByOrganisationId(int organisationId)
        {
            try
            {
                return await Mediator.Send(new GetContractTaskByOrgIdQuery { OrganisationId = organisationId , AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get contract task by organisationid was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpDelete("deletecontracttask/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> Delete(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteContractTaskCommand { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Task delete failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }
}
