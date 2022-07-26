using OnyxDoc.AuthService.Application.Roles.Commands.CreateRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using OnyxDoc.AuthService.Infrastructure.Utility;
using OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Roles.Commands.CreateRoleAndPermissions;
using OnyxDoc.AuthService.Application.Roles.Queries.GetRoles;
using OnyxDoc.AuthService.Application.Roles.Commands.ChangeRole;
using OnyxDoc.AuthService.Application.RolePermissions.Commands.UpdateRoleAndPermissions;
using OnyxDoc.AuthService.Application.Roles.Commands.UpdateRole;
using OnyxDoc.AuthService.Application.Common.Exceptions;

namespace OnyxDoc.AuthService.API.Controllers
{
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   
    public class RolesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public RolesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateRoleCommand command)
        {
            try
            {
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
                accessToken.ValidateToken(command.UserId,command.SubscriberId);
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
              
                if (roleId != command.RoleId || (roleId == 0 || command.RoleId == 0))
                {
                    return BadRequest("Invalid Role Id");
                }
                accessToken.ValidateToken(command.UserId,command.SubscriberId);
                return await Mediator.Send(command);
               
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

                accessToken.ValidateToken(command.UserId,command.SubscriberId);
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
                if (roleId != command.RoleId || (roleId == 0 || command.RoleId == 0))
                {
                    return BadRequest("Invalid Role Id");
                }
                accessToken.ValidateToken(command.UserId, command.SubscriberId);
                return await Mediator.Send(command);
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

  

        [HttpPost("changerolestatus")]
        public async Task<ActionResult<Result>> ChangeRoleStatus(ChangeRoleStatusCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId, command.SubscriberId);
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

        [HttpGet("getall/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(string userId, int skip, int take)
        {
            try
            {

                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetRolesQuery { Skip=skip, Take=take});
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



        [HttpGet("getbyid/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id,int subscriberId, string userId)
        {
            try
            {
               
                if (id <= 0)
                {
                    return BadRequest("Please input valid role Id");
                }
                accessToken.ValidateToken(userId,subscriberId);
                return await Mediator.Send(new GetRoleByIdQuery { Id = id, SubscriberId = subscriberId, UserId = userId });
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


        [HttpGet("getbysubscriberid/{subscriberId}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetBySubscriberId(int subscriberId,string userId, int skip, int take)
        {
            try
            {

                accessToken.ValidateToken(userId, subscriberId);
                return await Mediator.Send(new GetRolesBySubscriberIdQuery { SubscriberId = subscriberId, Skip=skip, Take=take });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Role by subscriber Id  was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyname/{name}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetByName(string name,string userId, int skip, int take)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetRoleByNameQuery { Name = name, Skip=skip, Take=take });
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
        [HttpGet("getrolesfordisplay/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetRolesForDisplay(int subscriberId)
        {
            try
            {
                return await Mediator.Send(new GetRolesForDisplayQuery {  SubscriberId = subscriberId });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Roles for display was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getallrolepermissions/{userId}")]
        public async Task<ActionResult<Result>> GetRolesPermission(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
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



        [HttpGet("getrolesandpermissionsbyroleid/{subscriberid}/{userId}/{roleId}")]
        public async Task<ActionResult<Result>> GetRoleAndPermissionsByRoleId(int subscriberId, string userId, int roleId)
        {
            try
            {
                accessToken.ValidateToken(userId, subscriberId);
                return await Mediator.Send(new GetRoleAndPermissionsByRoleIdQuery { SubscriberId= subscriberId, UserId = userId,RoleId = roleId});
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
