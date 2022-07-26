using Onyx.AuthService.Application.Roles.Commands.CreateRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Roles.Commands.UpdateRole;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Roles.Queries.GetRoles;
using Onyx.AuthService.Application.Roles.Commands.ChangeRole;
using Onyx.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using Onyx.AuthService.Application.RolePermissions.Commands.CreateRolePermissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.AuthService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Onyx.AuthService.Infrastructure.Utility;
using System.Linq;
using System;
using Onyx.AuthService.Infrastructure.Authorization;

namespace Onyx.AuthService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[OrgIdAuthFilter]
    public class RolePermissionsController : ApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RolePermissionsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("create/{userId}")]
        public async Task<ActionResult<Result>> Create(CreateRolePermissionsCommand command)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (command.UserId == null || command.UserId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "RolePermission creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{userId}")]
        public async Task<ActionResult<Result>> Update(UpdateRolePermissionsCommand command)
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
                if (command.UserId == null || command.UserId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (command.RoleId == 0)
                {
                    return BadRequest("Invalid Role Id");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "RolePermission update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyaccesslevel/{accessLevel}/{userId}")]
        public async Task<ActionResult<Result>> GetRolePermissions(int accessLevel, string userId)
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
                if (accessLevel <= 0)
                {
                    return BadRequest("Please input valid access level");
                }
                return await Mediator.Send(new GetRolePermissionsByAccessLevelQuery { AccessLevel = accessLevel });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving all RolePermission was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyroleid/{roleid}/{userId}")]
        public async Task<ActionResult<Result>> GetByRoleId(int roleid, string userId)
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
                if (roleid <= 0)
                {
                    return BadRequest("Please input valid role id");
                }
                return await Mediator.Send(new GetRolesPermissionByRoleIdQuery { RoleId = roleid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving RolePermission by Role Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{organizationid}")]
        public async Task<ActionResult<Result>> GetAllByOrganizationId(int organizationid, string userId)
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
                if (orgId != organizationid)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (organizationid <= 0 || organizationid != orgId)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetRolePermissionsQuery { OrganizationId = organizationid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving RolePermissions by Organization Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


    }
}
