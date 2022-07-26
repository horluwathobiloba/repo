using Onyx.AuthService.Application.Organizations.Commands.CreateOrganization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Organizations.Commands.UpdateOrganization;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Organizations.Commands.ChangeOrganizationStatus;
using Onyx.AuthService.Application.Organizations.Queries.GetOrganizations;
using System;
using static API.Filters.CustomAuthorizationFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.AuthService.Application.Organizations.Commands.SignUpOrganization;
using Onyx.AuthService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Onyx.AuthService.Infrastructure.Utility;
using Onyx.AuthService.Infrastructure.Authorization;

namespace Onyx.AuthService.API.Controllers
{
    // [Authorize]
    //  [CustomAuthenticationFilter]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[OrgIdAuthFilter]
    public class OrganizationsController : ApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrganizationsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("signup")]
        public async Task<ActionResult<Result>> Create([FromBody] SignUpOrganizationCommand command)
        {
            try
            {
               
                if (string.IsNullOrEmpty(command.Name))
                {
                    return BadRequest("Organization Name cannot be empty");
                }
                if (string.IsNullOrEmpty(command.RCNumber))
                {
                    return BadRequest("RC Number cannot be empty");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Organization signup was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("create/{userId}")]
        // [CustomAuthorize("Admin", "SuperAdmin")]
        public async Task<ActionResult<Result>> Create([FromBody] CreateOrganizationCommand command, string userId)
        {

            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrEmpty(command.Name))
                {
                    return BadRequest("Organization Name cannot be empty");
                }
                if (string.IsNullOrEmpty(command.RCNumber))
                {
                    return BadRequest("RC Number cannot be empty");
                }
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Organization creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{organizationId}/{userId}")]
        // [CustomAuthorize("Admin", "SuperAdmin")]
        public async Task<ActionResult<Result>> Update(int organizationId, UpdateOrganizationCommand command, string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != command.OrganizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (organizationId != command.OrganizationId || (organizationId == 0 || command.OrganizationId == 0))
                {
                    return BadRequest("Invalid Organization Id");
                }

                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Organization update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changeorganizationstatus/{userId}")]
        //[CustomAuthorize("Admin", "SuperAdmin")]
        public async Task<ActionResult<Result>> ChangeOrganizationStatus(ChangeOrganizationStatusCommand command, string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != command.OrganizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Changing Organization status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        // [CustomAuthorize("Admin", "SuperAdmin", "PowerUser", "SalesOfficer", "Support")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = accessToken.Claims.First(claim => claim.Type == "typ").Value;

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(new GetOrganizationsQuery());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving all Organizations was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id, string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenId = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenId)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (id <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetOrganizationByIdQuery { Id = id });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Organization by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyname/{name}/{userId}")]
        public async Task<ActionResult<Result>> GetByName(string name, string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Please input valid organization name");
                }
                return await Mediator.Send(new GetOrganizationByNameQuery { Name = name });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Organization by Name was not successful" + ex?.Message ?? ex?.InnerException?.Message });

            }
        }
    }

}
