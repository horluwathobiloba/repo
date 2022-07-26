using Onyx.WorkFlowService.Application.Departments.Commands.CreateDepartment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Departments.Commands.UpdateDepartment;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Application.Departments.Commands;
using Onyx.WorkFlowService.Application.Departments.Queries.GetDepartments;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Onyx.WorkFlowService.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepartmentsController : ApiController
    {
        [HttpPost("create")]
       // [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Result>> Create(CreateDepartmentCommand command)
        {
            try
            {
                if (command.Names == null || command.Names.Count == 0)
                {
                    return BadRequest("Department Name cannot be empty");
                }
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Department creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{departmentId}")]
      // [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Result>> Update(int departmentId, UpdateDepartmentCommand command)
        {
            try
            {
                if (departmentId != command.DepartmentId || (departmentId == 0 || command.DepartmentId == 0))
                {
                    return BadRequest("Invalid department Id ");
                }

                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Department update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changedepartmentstatus")]
       // [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Result>> ChangeDepartmentStatus(ChangeDepartmentStatusCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Changing Department status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall")]
        //[Authorize(Policy = "Admin")]
        public async Task<ActionResult<Result>> GetAll()
        {
            try
            {
                return await Mediator.Send(new GetDepartmentsQuery());
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Department retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("Please input valid department Id");
                }
                return await Mediator.Send(new GetDepartmentByIdQuery { Id = id });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieval by Department Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getbyorganizationid/{organizationid}")]
        public async Task<ActionResult<Result>> GetByOrganizationId(int organizationid)
        {
            try
            {
                if (organizationid <= 0)
                {
                    return BadRequest("Please input valid organizationid Id");
                }
                return await Mediator.Send(new GetDepartmentsByOrganizationIdQuery { OrganizationId = organizationid });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieval by Organization Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyname/{name}")]
        public async Task<ActionResult<Result>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Please input valid department name");
                }
                return await Mediator.Send(new GetDepartmentByNameQuery { Name = name });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieval by department name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
