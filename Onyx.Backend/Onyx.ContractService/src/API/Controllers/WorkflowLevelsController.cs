using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.WorkflowLevels.Commands.ChangeWorkflowLevelStatus;
using Onyx.ContractService.Application.WorkflowLevels.Commands.CreateWorkflowLevel;
using Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevel;
using Onyx.ContractService.Application.WorkflowLevels.Queries.GetWorkflowLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Application.WorkflowLevels.Commands.UpdateWorkflowLevels;
using Onyx.ContractService.Application.WorkflowLevels.Commands.CreateWorkflowLevels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WorkflowLevelsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public WorkflowLevelsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateWorkflowLevelCommand command)
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
                return Result.Failure($"Workflow level creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createworkflowlevels")]
        public async Task<ActionResult<Result>> CreateWorkflowLevels(CreateWorkflowLevelsCommand command)
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
                return Result.Failure($"Workflow level creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateWorkflowLevelCommand command)
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

                return Result.Failure($"Workflow level update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updateworkflowlevels")]
        public async Task<ActionResult<Result>> UpdateWorkflowLevels(UpdateWorkflowLevelsCommand command)
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

                return Result.Failure($"Workflow levels update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updateworkflowlevelstatus")]
        public async Task<ActionResult<Result>> UpdateWorkflowLevelStatus(UpdateWorkflowLevelStatusCommand command)
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

                return Result.Failure($"Changing workflow level status was not successful. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getworkflowlevels/{organisationid}")]
        public async Task<ActionResult<Result>> GetWorkflowLevels(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowLevelsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Workflow level retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getworkflowlevel/{id}")]
        public async Task<ActionResult<Result>> GetWorkflowLevel(int id)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowLevelByIdQuery { Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get workflow level by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getworkflowlevelsbyphase/{organisationid}/{workflowphaseid}")]
        public async Task<ActionResult<Result>> GetWorkflowLevelsByPhase(int organisationid, int workflowphaseid)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowLevelByPhaseIdQuery { OrganisationId = organisationid, WorkflowPhaseId = workflowphaseid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get workflow levels by phase failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpGet("getworkflowlevelsbyaction/{organisationid}/{action}")]
        public async Task<ActionResult<Result>> GetWorkflowLevelsByAction(int organisationid, WorkflowLevelAction action)
        {
            try
            {
                return await Mediator.Send(new GetWorkflowLevelsByActionQuery { OrganisationId = organisationid, WorkflowLevelAction = action, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get workflow levels by workflow level action failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
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

                return Result.Failure($"Get workflow levels by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

    }
}
