using Onyx.WorkFlowService.Application.Staffs.Commands.CreateStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Staffs.Commands.UpdateStaff;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Application.Staffs.Commands.ChangeStaffStatus;
using Onyx.WorkFlowService.Application.Staffs.Queries.GetStaffs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Onyx.WorkFlowService.API.Controllers
{
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StaffsController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateStaffCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Staff creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{staffId}")]
        public async Task<ActionResult<Result>> Update(string staffId, UpdateStaffCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(staffId))
                {
                    return BadRequest("Invalid Staff Id");
                }

                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Staff update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            
            }
        }

     
        [HttpPost("changestaffstatus")]
        public async Task<ActionResult<Result>> ChangeStaffStatus(ChangeStaffStatusCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Changing Staff Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall")]
        public async Task<ActionResult<Result>> GetAll()
        {
            try
            {
                return await Mediator.Send(new GetStaffsQuery());
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Staff retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Result>> GetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Please input valid staff Id");
                }
                return await Mediator.Send(new GetStaffByIdQuery { Id = id });
            }
            catch (System.Exception ex)

            {

                return Result.Failure(new string[] { "Staff retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
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
                return await Mediator.Send(new GetStaffsByOrganizationIdQuery { OrganizationId = organizationid });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Staff retrieval by organization id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getbydepartmentid/{departmentid}")]
        public async Task<ActionResult<Result>> GetByDepartmentId(int departmentid)
        {
            try
            {
                if (departmentid <= 0)
                {
                    return BadRequest("Please input valid department Id");
                }
                return await Mediator.Send(new GetStaffsByDepartmentIdQuery { DepartmentId = departmentid });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Staff retrieval by department id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyusername/{username}")]
        public async Task<ActionResult<Result>> GetByUserName(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest("Please input valid user name");
                }
                return await Mediator.Send(new GetStaffByUsernameQuery { UserName = username });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Staff retrieval by username was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
