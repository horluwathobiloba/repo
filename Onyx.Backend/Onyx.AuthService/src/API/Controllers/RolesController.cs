using Onyx.AuthService.Application.Roles.Commands.CreateRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Roles.Commands.UpdateRole;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Roles.Queries.GetRoles;
using Onyx.AuthService.Application.Roles.Commands.ChangeRole;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.AuthService.Application.Roles.Commands.ChangeRoleStatus;
using Onyx.AuthService.Application.RolePermissions.Commands.UpdateRoleAndPermissions;
using Onyx.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using Onyx.AuthService.Application.RolePermissions.Commands.CreateRolePermissions;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Roles.Commands.CreateRoleAndPermissions;
using Microsoft.AspNetCore.Http;
using Onyx.AuthService.Infrastructure.Utility;
using System.Linq;
using Onyx.AuthService.Infrastructure.Authorization;

namespace Onyx.AuthService.API.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[OrgIdAuthFilter]
    public class RolesController : ApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RolesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateRoleCommand command)
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
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Role creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("createroleandpermissions")]
        public async Task<ActionResult<Result>> CreateRoleAndPermissions(CreateRoleAndPermissionsCommand command)
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
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Role and permission creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpPost("update/{roleId}")]
        public async Task<ActionResult<Result>> Update(int roleId, UpdateRoleCommand command)
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
                if (roleId != command.RoleId || (roleId == 0 || command.RoleId == 0))
                {
                    return BadRequest("Invalid Role Id");
                }

                return await Mediator.Send(command);
                {

                }
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Role update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }

        [HttpPost("updateroleandpermissions")]
        public async Task<ActionResult<Result>> UpdateRoleAndPermissions(UpdateRoleAndPermissionsCommand command)
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
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Role and permission update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("update/permission")]
        public async Task<ActionResult<Result>> Update(int roleId, UpdateRoleAndPermissionsCommand command)
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
                if (roleId != command.RoleId || (roleId == 0 || command.RoleId == 0))
                {
                    return BadRequest("Invalid Role Id");
                }

                return await Mediator.Send(command);
                {

                }
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Permission update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }

        //[HttpPost("date/permission")]
        //public async Task<ActionResult<Result>> Update1(int acccessLevelId, UpdateRolePermissionsAccessLevelCommand command)
        //{
        //    try
        //    {


        //        if (acccessLevelId != Enum.GetValues((AccessLevel)command.AccessLevel) || (acccessLevelId == 0 || (int)command.AccessLevel == 0))
        //        {
        //            return BadRequest("Invalid Role Id");
        //        }

        //        if (acccessLevelId != (int)command.AccessLevel || (acccessLevelId == 0 || (int)command.AccessLevel == 0))
        //        {
        //            return BadRequest("Invalid Role Id");
        //        }
        //        (AccessLevel)request.AccessLevel

        //        return await Mediator.Send(command);
        //        {
        //            {

        //            }
        //        }
        //    }

        //    catch (ValidationException ex)
        //    {
        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.Value}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure(new string[] { "Permission update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }

        //}

        [HttpPost("changerolestatus")]
        public async Task<ActionResult<Result>> ChangeRoleStatus(ChangeRoleStatusCommand command)
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
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Changing Role status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {  
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(new GetRolesQuery());
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving  Roles was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id,string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (id <= 0)
                {
                    return BadRequest("Please input valid role Id");
                }
                //if (string.IsNullOrWhiteSpace(id))
                //{
                //    return BadRequest("Please input valid role Id");
                //}

                return await Mediator.Send(new GetRoleByIdQuery { Id = id });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Role by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyorganizationid/{organizationid}/{userId}")]
        public async Task<ActionResult<Result>> GetByOrganizationId(int organizationid,string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;
               
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != organizationid)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (organizationid <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetRolesByOrganizationIdQuery { OrganizationId = organizationid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Role by organization Id  was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyname/{name}/{userId}")]
        public async Task<ActionResult<Result>> GetByName(string name,string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Please input valid role name");
                }
                return await Mediator.Send(new GetRoleByNameQuery { Name = name });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Role by name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallrolepermissions/{userId}")]
        public async Task<ActionResult<Result>> GetRolesPermission(string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(new GetRolePermissionsQuery());
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving  all role permissions was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getrolepermissionsbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetRolePermissionById(int id,string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (id <= 0)
                {
                    return BadRequest("Please input valid role Id");
                }

                return await Mediator.Send(new GetRolesPermissionByRoleIdQuery { RoleId = id });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Permissions  by Role Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
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
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
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

                return Result.Failure(new string[] { "Retrieving all Role Permission was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
