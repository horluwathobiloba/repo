using Onyx.WorkFlowService.Application.Organizations.Commands.CreateOrganization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Organizations.Commands.UpdateOrganization;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Application.Organizations.Commands.ChangeOrganizationStatus;
using Onyx.WorkFlowService.Application.Organizations.Queries.GetOrganizations;
using System;
using static API.Filters.CustomAuthorizationFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Onyx.WorkFlowService.API.Controllers
{
    // [Authorize]
    //  [CustomAuthenticationFilter]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrganizationsController : ApiController
    {
        [HttpPost("create")]
       // [CustomAuthorize("Admin", "SuperAdmin")]
        public async Task<ActionResult<Result>> Create([FromBody]CreateOrganizationCommand command)
        {
            
                try
                {
                    if (string.IsNullOrEmpty(command.Name))
                    {
                        return BadRequest("Organization Name cannot be empty");
                    }
                    return await Mediator.Send(command);
                }
                catch (Exception ex)
                {

                    return Result.Failure(new string[] { "Organization creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
                }    
        }

        [HttpPost("update/{organizationId}")]
       // [CustomAuthorize("Admin", "SuperAdmin")]
        public async Task<ActionResult<Result>> Update(int organizationId, UpdateOrganizationCommand command)
        {
            try
            {
                if (organizationId != command.OrganizationId || (organizationId == 0 || command.OrganizationId == 0))
                {
                    return BadRequest("Invalid Organization Id");
                }

                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Organization update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changeorganizationstatus")]
       // [CustomAuthorize("Admin", "SuperAdmin")]
        public async Task<ActionResult<Result>> ChangeOrganizationStatus(ChangeOrganizationStatusCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Changing Organization status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall")]
      //  [CustomAuthorize("Admin", "SuperAdmin", "PowerUser", "SalesOfficer", "Support")]
        public async Task<ActionResult<Result>> GetAll()
        {
            try
            {
                return await Mediator.Send(new GetOrganizationsQuery());
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving all Organizations was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetOrganizationByIdQuery { Id = id });
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Organization by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyname/{name}")]
        public async Task<ActionResult<Result>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Please input valid organization name");
                }
                return await Mediator.Send(new GetOrganizationByNameQuery { Name = name });
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Organization by Name was not successful" + ex?.Message ?? ex?.InnerException?.Message });

            }
        }
    }

}
