using Onyx.WorkFlowService.Application.Roles.Commands.CreateRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Roles.Commands.UpdateRole;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Application.Roles.Queries.GetRoles;
using Onyx.WorkFlowService.Application.Roles.Commands.ChangeRole;
using Onyx.WorkFlowService.Application.RolePermissions.Queries.GetRolePermissions;
using Onyx.WorkFlowService.Application.RolePermissions.Commands.CreateRolePermissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Onyx.WorkFlowService.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolePermissionsController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateRolePermissionsCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "RolePermission creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateRolePermissionsCommand command)
        {
            try
            {
                if (command.RoleId == 0)
                {
                    return BadRequest("Invalid Role Id");
                }
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "RolePermission update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyaccesslevel/{accessLevel}")]
        public async Task<ActionResult<Result>> GetRolePermissions(int accessLevel)
        {
            try
            {
                if (accessLevel <= 0)
                {
                    return BadRequest("Please input valid access level");
                }
                return await Mediator.Send(new GetRolePermissionsByAccessLevelQuery { AccessLevel = accessLevel });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving all RolePermission was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

     
        [HttpGet("getbyroleid/{roleid}")]
        public async Task<ActionResult<Result>> GetByRoleId(int roleid)
        {
            try
            {
                if (roleid <= 0)
                {
                    return BadRequest("Please input valid role id");
                }
                return await Mediator.Send(new GetRolesPermissionByRoleIdQuery { RoleId = roleid });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving RolePermission by Role Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{organizationid}")]
        public async Task<ActionResult<Result>> GetAllByOrganizationId(int organizationid)
        {
            try
            {
                if (organizationid <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetRolePermissionsQuery { OrganizationId = organizationid });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving RolePermissions by Organization Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

      
    }
}
