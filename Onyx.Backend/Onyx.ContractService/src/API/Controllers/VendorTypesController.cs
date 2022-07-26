using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.VendorTypes.Commands.CreateVendorType;
using Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorType;
using Onyx.ContractService.Application.VendorTypes.Queries.GetVendorTypes;
using Onyx.ContractService.Application.Common.Exceptions;
using System.Threading.Tasks;
using Onyx.ContractService.Application.VendorTypes.Commands.CreateVendorTypes;
using Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorTypeStatus;
using Onyx.ContractService.Application.VendorTypes.Commands.UpdateVendorTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Infrastructure.Utility;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VendorTypesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public VendorTypesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateVendorTypeCommand command)
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

        [HttpPost("createvendortypes")]
        public async Task<ActionResult<Result>> CreateVendorTypes(CreateVendorTypesCommand command)
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
        public async Task<ActionResult<Result>> Update(UpdateVendorTypeCommand command)
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


        [HttpPost("updatevendortypes")]
        public async Task<ActionResult<Result>> UpdateVendorTypes(UpdateVendorTypesCommand command)
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

        [HttpPost("updatevendortypestatus")]
        public async Task<ActionResult<Result>> UpdateVendorTypeStatus(UpdateVendorTypeStatusCommand command)
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

        [HttpGet("getvendortypes/{organisationid}")]
        public async Task<ActionResult<Result>> GetVendorTypes(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetVendorTypesQuery { OrganisationId = organisationid, AccessToken = accessToken });
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

        [HttpGet("getvendortypebyid/{id}")]
        public async Task<ActionResult<Result>> GetVendorTypeById(int id)
        {
            try
            {
                return await Mediator.Send(new GetVendorTypeByIdQuery { Id = id, AccessToken = accessToken });
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

        [HttpGet("getvendortypesbyname/{organisationid}/{name}")]
        public async Task<ActionResult<Result>> GetVendorTypesByName(int organisationid, string name)
        {
            try
            {
                return await Mediator.Send(new GetVendorTypesByNameQuery { OrganisationId = organisationid, Name = name, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get vendor types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetVendorTypesDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get vendor types by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
