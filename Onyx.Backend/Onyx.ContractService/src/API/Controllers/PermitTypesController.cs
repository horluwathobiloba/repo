using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.PermitTypes.Commands.CreatePermitType;
using Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitType;
using Onyx.ContractService.Application.PermitTypes.Queries.GetPermitTypes;
using Onyx.ContractService.Application.Common.Exceptions;
using System.Threading.Tasks;
using Onyx.ContractService.Application.PermitTypes.Commands.CreatePermitTypes;
using Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitTypeStatus;
using Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Infrastructure.Utility;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PermitTypesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PermitTypesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You're not authorized");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreatePermitTypeCommand command)
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
                return Result.Failure($"Vendor type creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createpermittypes")]
        public async Task<ActionResult<Result>> CreatePermitTypes(CreatePermitTypesCommand command)
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
                return Result.Failure($"Vendor types creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdatePermitTypeCommand command)
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

                return Result.Failure($"Vendor type creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


        [HttpPost("updatepermittypes")]
        public async Task<ActionResult<Result>> UpdatePermitTypes(UpdatePermitTypesCommand command)
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

                return Result.Failure($"Vendor types creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatepermittypestatus")]
        public async Task<ActionResult<Result>> UpdatePermitTypeStatus(UpdatePermitTypeStatusCommand command)
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

                return Result.Failure($"Changing Vendor type status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getpermittypes/{organisationid}")]
        public async Task<ActionResult<Result>> GetPermitTypes(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetPermitTypesQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Vendor type retrieval failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getpermittypebyid/{id}")]
        public async Task<ActionResult<Result>> GetPermitTypeById(int id)
        {
            try
            {
                return await Mediator.Send(new GetPermitTypeByIdQuery { Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Vendor type by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getpermittypesbyname/{organisationid}/{name}")]
        public async Task<ActionResult<Result>> GetPermitTypesByName(int organisationid, string name)
        {
            try
            {
                return await Mediator.Send(new GetPermitTypesByNameQuery { OrganisationId = organisationid, Name = name, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get permit types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetPermitTypesDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get permit types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
