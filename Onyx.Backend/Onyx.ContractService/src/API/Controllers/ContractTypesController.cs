using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models; 
using Onyx.ContractService.Application.ContractTypes.Commands.ChangeContractTypeStatus;
using Onyx.ContractService.Application.ContractTypes.Commands.CreateContractType;
using Onyx.ContractService.Application.ContractTypes.Commands.UpdateContractType;
using Onyx.ContractService.Application.ContractTypes.Queries.GetContractTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.ContractTypes.Commands.UpdateContractTypeAndInitiators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractTypesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractTypesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractTypeCommand command)
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
                return Result.Failure($"Contract type creation was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateContractTypeAndInitiatorsCommand command)
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

                return Result.Failure($"Contract type update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontracttype")]
        public async Task<ActionResult<Result>> UpdateContractType(UpdateContractTypeCommand command)
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

                return Result.Failure($"Contract type update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        } 

        [HttpPost("updatecontracttypestatus")]
        public async Task<ActionResult<Result>> UpdateContractTypeStatus(UpdateContractTypeStatusCommand command)
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

                return Result.Failure($"Changing contract type status was not successful. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontracttypes/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractTypes(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractTypesQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract type retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttypebyid/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetContractTypeById(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetContractTypeByIdQuery { OrganisationId = organisationid,  Id = id, AccessToken =  accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get contract type by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttypesbyname/{organisationid}/{name}")]
        public async Task<ActionResult<Result>> GetContractTypesByName(int organisationid, string name)
        {
            try
            {
                return await Mediator.Send(new GetContractTypesByNameQuery { OrganisationId = organisationid, Name = name, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get contract types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetContractTypesDynamicQuery { OrganisationId = organisationid, SearchText = searchtext,  AccessToken =  accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get contract types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }     
    }
}
