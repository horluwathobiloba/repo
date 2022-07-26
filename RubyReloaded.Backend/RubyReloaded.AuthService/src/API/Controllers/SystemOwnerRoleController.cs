using MediatR;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSystemOwnerRole;
using System;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSsystemOwnerRoleAndPermission;
using RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.UpdateSystemOwnerRole;
using RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.ChangeSystemRoleStatusCommand;
using RubyReloaded.AuthService.Application.SystemOwnerRoles.Queries.GetSystemOwnerRoles;
using RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRoleAndPermissons;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.AuthService.Application.SystemOwnerRolePermission.Queries.GetSystemOwnerRolePermissions;

namespace API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SystemOwnerRoleController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSystemOwnerRoleCommand command)
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
        [HttpPost("createroleandpermission")]
        public async Task<ActionResult<Result>> createroleandpermission(CreateSystemOwnerRoleAndPermissionCommand command)
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
        //[HttpPost("createsystemownerroleandpermissions")]
        //public async Task<ActionResult<Result>> CreateSystemOwnerRoleAndPermissions(CreateSystemOwnerRoleAndPermissionCommand command)
        //{
        //    try
        //    {
        //        return await Mediator.Send(command);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure(new string[] { "Role and permission creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}

        [HttpPost("update/{roleid}")]
        public async Task<ActionResult<Result>> Update(int roleId, UpdateSystemOwnerRoleCommand command)
        {
            try
            {
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


        [HttpPost("changesystemownerrolestatus")]
        public async Task<ActionResult<Result>> ChangeSystemOwnerRoleStatus(ChangeSystemRoleStatusCommand command)
        {
            try
            {
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

        [HttpGet("getall/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(int skip, int take)
        {
            try
            {
                return await Mediator.Send(new GetAllSystemOwnerRoles
                {
                    Take = take,
                    Skip = skip
                });
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

        [HttpGet("getuserrolepermissions")]
        public async Task<ActionResult<Result>> GetSystemUserRolePermissions(string email, int systemowneruserId)
        {
            try
            {
                return await Mediator.Send(new GetSystemOwnerRolePermissions
                {
                    SystemOwnerUserId= systemowneruserId,
                    Email = email
                });
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



    }
}
