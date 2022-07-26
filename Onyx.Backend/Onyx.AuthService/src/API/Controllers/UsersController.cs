using Onyx.AuthService.Application.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Users.Commands.UpdateUser;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Users.Commands.ChangeUserStatus;
using Onyx.AuthService.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Customers.Commands.VerifyEmail;
using System;
using Onyx.AuthService.Application.Users.Commands.SendContractRequestEmail;
using Microsoft.AspNetCore.Http;
using Onyx.AuthService.Infrastructure.Utility;
using System.Linq;
using Onyx.AuthService.Application.Users.Queries.GetStaffs;
using Onyx.AuthService.Infrastructure.Authorization;

namespace Onyx.AuthService.API.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[OrgIdAuthFilter]
    public class UsersController : ApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateUserCommand command)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

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

                return Result.Failure(new string[] { "User creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{userId}")]
        public async Task<ActionResult<Result>> Update(string userId, UpdateUserCommand command)
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
                if (command.UserId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("Invalid User Id");
                }

                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changeuserstatus")]
        public async Task<ActionResult<Result>> ChangeUserStatus(ChangeUserStatusCommand command)
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
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Changing User Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}")]
        public async Task<ActionResult<Result>> GetAll(string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                return await Mediator.Send(new GetUsersQuery());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("getallusers")]
        public async Task<ActionResult<Result>> GetAllUsers()
        {
            try
            {
                return await Mediator.Send(new GetUsersQuery());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getByJobFunction/{jobfunctionId}/{userId}/{orgId}")]
        public async Task<ActionResult<Result>> GetUsersByJobFunctionAndOrganization(int? jobfunctionId, string userId, int? orgId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;
                var OrgId = Convert.ToInt32(accessToken.Claims.First(claim => claim.Type == "organizationId").Value);
                if (orgId != OrgId)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (jobfunctionId is null || orgId is null)
                {
                    return BadRequest($"parameters cannot be null");
                }
                return await Mediator.Send(new GetUserByJobFunction { OrgId = orgId, JobfunctionId = jobfunctionId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("emailconfirmation")]
        public async Task<ActionResult<Result>> EmailConfirmation([FromQuery] VerifyEmailCommand command)
        {
            try
            {

                if (string.IsNullOrEmpty(command.Email))
                    return BadRequest("Invalid Email Account");
                return await Mediator.Send(command);

            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Sending Email Confirmation was unsuccessful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(string id, string userId)
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
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Please input valid user Id");
                }
                return await Mediator.Send(new GetUserByIdQuery { Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyorganizationid/{organizationid}/{userId}")]
        public async Task<ActionResult<Result>> GetByOrganizationId(int organizationid, string userId)
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
                if (orgId != organizationid)
                {
                    return BadRequest("You're not authorized");
                }

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (organizationid <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetUsersByOrganizationIdQuery { OrganizationId = organizationid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval by organization id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbydepartmentid/{departmentid}/{userId}")]
        public async Task<ActionResult<Result>> GetByDepartmentId(int departmentid, string userId)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (departmentid <= 0)
                {
                    return BadRequest("Please input valid department Id");
                }
                return await Mediator.Send(new GetUsersByDepartmentIdQuery { DepartmentId = departmentid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "User retrieval by department id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyusername/{username}/{userId}")]
        public async Task<ActionResult<Result>> GetByUserName(string username, string userId)
        {
            try
            {

                var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
                if (accessToken == null)
                {
                    return BadRequest("You're not authorized");
                }
                var id = accessToken.Claims.First(claim => claim.Type == "userid").Value;

                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest("Please input valid user name");
                }
                return await Mediator.Send(new GetUserByUsernameQuery { UserName = username });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "User retrieval by username was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontractgenerators/{organizationId}/{userId}")]
        public async Task<ActionResult<Result>> GetContractGenerators(int organizationId, string userId)
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
                if (orgId != organizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("Please input valid user Id");
                }
                if (organizationId <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetUserByContractGenPermissionsQuery { UserId = userId, OrganizationId = organizationId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Contract Generators retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontractinternalsignatories/{organizationId}/{userId}")]
        public async Task<ActionResult<Result>> GetContractInternalSignatories(int organizationId, string userId)
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
                if (orgId != organizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("Please input valid user Id");
                }
                if (organizationId <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetUserBySignAndAcceptPermissionsQuery { UserId = userId, OrganizationId = organizationId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Contract Internal Signatories retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getcontractapprovers/{organizationId}/{userId}")]
        public async Task<ActionResult<Result>> GetContractApprovers(int organizationId, string userId)
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
                if (orgId != organizationId)
                {
                    return BadRequest("You're not authorized");
                }
                if (userId == null || userId != id)
                {
                    return BadRequest("Invalid Token Credentials");
                }
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("Please input valid user Id");
                }
                if (organizationId <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetUserByContractApproverPermissionsQuery { UserId = userId, OrganizationId = organizationId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Contract Approvers retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("sendcontractrequestemail/{userId}")]
        public async Task<ActionResult<Result>> SendContractRequestEmail(SendContractRequestEmailCommand command, string userId)
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

                if (userId == null || userId != id)
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

                return Result.Failure(new string[] { "Send Approval email was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
