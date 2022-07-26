using Onyx.WorkFlowService.Application.Roles.Commands.CreateRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Roles.Commands.UpdateRole;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Application.Roles.Queries.GetRoles;
using Onyx.WorkFlowService.Application.Roles.Commands.ChangeRole;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Onyx.WorkFlowService.API.Controllers
{
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateRoleCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Role creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
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

                return await Mediator.Send(command);
                {

                }
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
                return await Mediator.Send(new GetRoleQuery());
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving  Roles was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Result>> GetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Please input valid role Id");
                }
                
                return await Mediator.Send(new GetRoleByIdQuery { Id = id });
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
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Role by name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
