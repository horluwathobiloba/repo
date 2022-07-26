using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.LicenseTypes.Commands.CreateLicenseType;
using Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseType;
using Onyx.ContractService.Application.LicenseTypes.Queries.GetLicenseTypes;
using Onyx.ContractService.Application.Common.Exceptions;
using System.Threading.Tasks;
using Onyx.ContractService.Application.LicenseTypes.Commands.CreateLicenseTypes;
using Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseTypeStatus;
using Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Infrastructure.Utility;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LicenseTypesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public LicenseTypesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You're not authorized");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateLicenseTypeCommand command)
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
                return Result.Failure($"License type creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createlicensetypes")]
        public async Task<ActionResult<Result>> CreateLicenseTypes(CreateLicenseTypesCommand command)
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
                return Result.Failure($"License types creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateLicenseTypeCommand command)
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

                return Result.Failure($"License type creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


        [HttpPost("updatelicensetypes")]
        public async Task<ActionResult<Result>> UpdateLicenseTypes(UpdateLicenseTypesCommand command)
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

                return Result.Failure($"License types creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatelicensetypestatus")]
        public async Task<ActionResult<Result>> UpdateLicenseTypeStatus(UpdateLicenseTypeStatusCommand command)
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

                return Result.Failure($"Changing License type status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getlicensetypes/{organisationid}")]
        public async Task<ActionResult<Result>> GetLicenseTypes(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetLicenseTypesQuery { OrganisationId = organisationid, AccessToken = accessToken });
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

        [HttpGet("getlicensetypebyid/{id}")]
        public async Task<ActionResult<Result>> GetLicenseTypeById(int id)
        {
            try
            {
                return await Mediator.Send(new GetLicenseTypeByIdQuery { Id = id, AccessToken = accessToken });
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

        [HttpGet("getlicensetypesbyname/{organisationid}/{name}")]
        public async Task<ActionResult<Result>> GetLicenseTypesByName(int organisationid, string name)
        {
            try
            {
                return await Mediator.Send(new GetLicenseTypesByNameQuery { OrganisationId = organisationid, Name = name, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get license types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetLicenseTypesDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get license types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
