using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiators;
using Onyx.ContractService.Application.ContractTypeInitiators.Commands.CreateContractTypeInitiators;
using Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiator;
using Onyx.ContractService.Application.ContractTypeInitiators.Queries.GetContractTypeInitiators;  
using Onyx.ContractService.Application.ContractTypeInitiators.Commands.CreateContractTypeInitiator;
using Onyx.ContractService.Application.ContractTypeInitiators.Commands.UpdateContractTypeInitiatorStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractTypeInitiatorsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractTypeInitiatorsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }


        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractTypeInitiatorCommand command)
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
                return Result.Failure($"Contract type Initiator creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcontracttypeinitiators")]
        public async Task<ActionResult<Result>> CreateContractTypeInitiators(CreateContractTypeInitiatorsCommand command)
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
                return Result.Failure($"Contract type Initiator creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateContractTypeInitiatorCommand command)
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

                return Result.Failure($"Contract type Initiator update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontracttypeinitiators")]
        public async Task<ActionResult<Result>> UpdateContractTypeInitiators(UpdateContractTypeInitiatorsCommand command)
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

                return Result.Failure($"Contract type Initiators update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontracttypeinitiatorstatus")]
        public async Task<ActionResult<Result>> UpdateContractTypeInitiatorstatus(UpdateContractTypeInitiatorStatusCommand command)
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

                return Result.Failure($"Changing Contract type Initiator status was not successful. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontracttypeinitiators/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractTypeInitiators(int organisationid)
        {
            try
            { 
                return await Mediator.Send(new GetContractTypeInitiatorsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract type Initiator retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttypeinitiator/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetContractTypeInitiator(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetContractTypeInitiatorQuery { Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Contract type Initiator by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttypeinitiatorsbyphase/{organisationid}/{contracttypeid}")]
        public async Task<ActionResult<Result>> GetContractTypeInitiatorsByPhase(int organisationid, int ContractTypeid)
        {
            try
            {
                return await Mediator.Send(new GetContractTypeInitiatorsByContractTypeQuery { OrganisationId = organisationid, ContractTypeId = ContractTypeid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contract type Initiators by phase failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpGet("getcontracttypeinitiatorsbyaction/{organisationid}/{roleid}")]
        public async Task<ActionResult<Result>> GetContractTypeInitiatorsByRole(int organisationid, int roleid)
        {
            try
            {
                return await Mediator.Send(new GetContractTypeInitiatorsByRoleQuery { OrganisationId = organisationid,  RoleId = roleid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contract type Initiators by role failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetContractTypeInitiatorsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contract type Initiators by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

    }
}
