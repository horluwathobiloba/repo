using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRole;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRoleAndPermissons;
using RubyReloaded.AuthService.Application.AjoRole.Commands.UpdateAjoRole;
using RubyReloaded.AuthService.Application.AjoRole.Queries.GetAjoRoles;
using RubyReloaded.AuthService.Application.AjoRole.Commands.ChangeAjoRoleStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AjoRoleController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateAjoRoleCommand command)
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
        public async Task<ActionResult<Result>> Testing(CreateRoleAndPermissonAjoCommand command)
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
        //[HttpPost("createpermissions")]
        //public async Task<ActionResult<Result>> CreateAjoRoleAndPermissions(CreateAjoRoleAndPermissonCommand command)
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
        public async Task<ActionResult<Result>> Update(int roleId, UpdateAjoRoleCommand command)
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


        [HttpPost("changeajorolestatus")]
        public async Task<ActionResult<Result>> ChangeAjoRoleStatus(ChangeAjoRoleStatusCommand command)
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

        [HttpGet("getall")]
        public async Task<ActionResult<Result>> GetAll()
        {
            try
            {
                return await Mediator.Send(new GetAjoRolesQuery());
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

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {

                return await Mediator.Send(new GetAjoRoleById { RoleId = id });
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


    }
}
