using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowPhases.Commands.ChangeWorkflowPhaseStatus;
using Onyx.ContractService.Application.WorkflowPhases.Commands.CreateWorkflowPhase;
using Onyx.ContractService.Application.WorkflowPhases.Commands.UpdateWorkflowPhase;
using Onyx.ContractService.Application.WorkflowPhases.Queries.GetWorkflowPhases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.WorkflowPhases.Commands.CreateWorkflowPhaseAndLevels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WorkflowPhasesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WorkflowPhasesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateWorkflowPhaseCommand command)
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
                return Result.Failure($"Workflow phase creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createworkflowphaseandlevels")]
        public async Task<ActionResult<Result>> CreateWorkflowPhaseAndLevels(CreateWorkflowPhaseAndLevelsCommand command)
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
                return Result.Failure($"Workflow phase and levels creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateWorkflowPhaseCommand command)
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

                return Result.Failure($"Workflow phase update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updateworkflowphasestatus")]
        public async Task<ActionResult<Result>> UpdateWorkflowPhaseStatus(UpdateWorkflowPhaseStatusCommand command)
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

                return Result.Failure($"Changing workflow phase status was not successful. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getworkflowphases/{organisationid}")]
        public async Task<ActionResult<Result>> GetWorkflowPhases(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowPhasesQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Workflow phase retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getworkflowphasebyid/{id}")]
        public async Task<ActionResult<Result>> GetWorkflowPhaseById(int id)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowPhaseByIdQuery { Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get workflow phase by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getworkflowphasesbyname/{organisationid}/{name}")]
        public async Task<ActionResult<Result>> GetWorkflowPhasesByName(int organisationid, string name)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowPhasesByNameQuery { OrganisationId = organisationid, Name = name, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get workflow phases by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowLevelsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get workflow phases by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

    }
}
