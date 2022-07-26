using OnyxDoc.AuthService.Application.Roles.Commands.CreateRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Roles.Commands.UpdateRole;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Roles.Queries.GetRoles;
using OnyxDoc.AuthService.Application.Roles.Commands.ChangeRole;
using OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using OnyxDoc.AuthService.Application.RolePermissions.Commands.CreateRolePermissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using OnyxDoc.AuthService.Infrastructure.Utility;
using System.Linq;
using System;
using OnyxDoc.AuthService.Infrastructure.Authorization;
using OnyxDoc.AuthService.Application.Roles.Commands.CreateDefaultRoleAndPermissions;

namespace OnyxDoc.AuthService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolePermissionsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public RolePermissionsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create/{userId}")]
        public async Task<ActionResult<Result>> Create(CreateRolePermissionsCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
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
        [HttpPost("createdefaultrole/{userId}")]
        public async Task<ActionResult<Result>> CreateDefaultRole(CreateDefaultRoleAndPermissionsCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Default Role and Permission creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{userId}")]
        public async Task<ActionResult<Result>> Update(UpdateRolePermissionsCommand command)
        {
            try
            {
               
                if (command.RoleId == 0)
                {
                    return BadRequest("Invalid Role Id");
                }
                accessToken.ValidateToken(command.UserId);
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
                if (accessLevel <= 0)
                {
                    return BadRequest("Please input valid access level");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetRolePermissionsByAccessLevelQuery { AccessLevel = accessLevel });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Role Permission by Access Level was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyroleid/{roleid}/{userId}")]
        public async Task<ActionResult<Result>> GetByRoleId(int roleid, string userId)
        {

            try
            {
                if (roleid <= 0)
                {
                    return BadRequest("Please input valid role id");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetRolesPermissionByRoleIdQuery { RoleId = roleid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Role Permission by Role Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{subscriberId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAllBySubscriberId(int subscriberId, string userId, int skip, int take)
        {
            try
            {
                accessToken.ValidateToken(userId, subscriberId);
                return await Mediator.Send(new GetRolePermissionsQuery { SubscriberId = subscriberId, Skip=skip, Take=take });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving All Role Permissions by Subscriber Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


    }
}
