using Microsoft.AspNetCore.Mvc;
using Onyx.AuthService.API.Controllers;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.JobFunctions.Commands;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.JobFunctions.Queries.GetJobFunction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Onyx.AuthService.Infrastructure.Utility;
using Onyx.AuthService.Infrastructure.Authorization;
using Onyx.AuthService.Application.JobFunctions.Commands.CreateJobFunctions;
using Onyx.AuthService.Application.JobFunctions.Commands.ChangeJobFunctionStatus;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [OrgIdAuthFilter]
    public class JobFunctionsController : ApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
       

        public JobFunctionsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateJobFunctionCommand command)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);

                if (orgId!=command.OrganisationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (command.UserId == null || command.UserId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Jobfunction creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("createjobfunctions")]
        public async Task<ActionResult<Result>> CreateJobFunctions(CreateJobFunctionsCommand command)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != command.OrganisationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (command.UserId == null || command.UserId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Jobfunction creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateJobFunctionCommand command)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != command.OrganizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (command.UserId == null || command.UserId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (command.Id == 0)
                {
                    return BadRequest("Invalid job Id");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Job update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changejobfunctionstatus")]
        public async Task<ActionResult<Result>> ChangeJobFunctionStatusCommand(ChangeJobFunctionStatusCommand command)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != command.OrganizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (command.UserId == null || command.UserId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (command.JobFunctionId == 0)
                {
                    return BadRequest("Invalid job Id");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Job update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyId/{id}/{userId}/{organizationId}")]
        public async Task<ActionResult<Result>> GetJob(int id,string userId,int organizationId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (id <= 0)
                {
                    return BadRequest("Please input valid id");
                }
                return await Mediator.Send(new GetJobFunctionById { Id = id,UserId=userId,OrganizationId= organizationId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving job was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{organizationid}/{userId}")]
        public async Task<ActionResult<Result>> GetAllByOrganizationId(int organizationid,string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var tokenid = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var orgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != organizationid)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != tokenid)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (organizationid <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetJobFunctionByOrgId { OrgId = organizationid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no elements")
            {
                return Result.Failure(ex.Message);
            }
            catch (System.Exception ex)
            {
               return Result.Failure(new string[] { "Retrieving jobs by Organization Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
