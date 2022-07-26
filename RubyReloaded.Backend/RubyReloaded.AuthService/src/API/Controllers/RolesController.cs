﻿using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Roles.Command.CreateRole;
using System;
using System.Collections.Generic;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Roles.Command.CreateRoleAndPermission;
using RubyReloaded.AuthService.Application.Roles.Command.UpdateRole;
using RubyReloaded.AuthService.Application.Roles.Commands.ChangeRole;
using RubyReloaded.AuthService.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController: ApiController
    {
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
        public async Task<ActionResult<Result>> CreateAndPermissions(CreateRoleAndPermissionCommand command)
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

                return Result.Failure(new string[] { "Role and permission creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{roleid}")]
        public async Task<ActionResult<Result>> Update(int roleId, UpdateRoleCommand command)
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


        [HttpPost("changerolestatus")]
        public async Task<ActionResult<Result>> ChangeRoleStatus(ChangeRoleStatusCommand command)
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

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {
            
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
        [HttpGet("getbyorganizationid/{organizationid}")]
        public async Task<ActionResult<Result>> GetByOrganizationId(int organizationid)
        {
            try
            {
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

        [HttpGet("getbyname/{name}")]
        public async Task<ActionResult<Result>> GetByName(string name)
        {
            try
            {
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

    }
}
