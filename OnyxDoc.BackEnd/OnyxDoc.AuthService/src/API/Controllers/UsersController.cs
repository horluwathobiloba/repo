using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Users.Commands.CreateUser;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Infrastructure.Utility;
using OnyxDoc.AuthService.Application.Users.Commands.UpdateUser;
using OnyxDoc.AuthService.Application.Users.Commands.ChangeUserStatus;
using OnyxDoc.AuthService.Application.Users.Commands.VerifyEmail;
using OnyxDoc.AuthService.Application.Users.Queries.GetUsers;
using OnyxDoc.AuthService.Application.SignUp;
using OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers;
using OnyxDoc.AuthService.Application.Commands.SignUp;

namespace OnyxDoc.AuthService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateUserCommand command)
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

                return Result.Failure(new string[] { "User creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("checkupdomainname")]
        public async Task<ActionResult<Result>> CheckUpDomainName(CheckUpDomainNameCommand command)
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

                return Result.Failure(new string[] { "Domain name check was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("editinviteduser")]
        public async Task<ActionResult<Result>> EditInvitedUser(EditInvitedUserCommand command)
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

                return Result.Failure(new string[] { "Edit invited user was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("joinexistingorganization")]
        public async Task<ActionResult<Result>> JoinExistingOrganization(JoinExistingOrganizationCommand command)
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

                return Result.Failure(new string[] { "Join existing organization was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("approveorrejectuserrequest")]
        public async Task<ActionResult<Result>> ApproveOrReject(ApproveOrRejectRequestCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Join existing organization was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        } 

        [HttpPost("signup")]
        public async Task<ActionResult<Result>> SignUp(SignUpCommand command)
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

                return Result.Failure(new string[] { "SignUp was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{userId}")]
        public async Task<ActionResult<Result>> Update(string userId, UpdateUserCommand command)
        {
            try
            {
              
                accessToken.ValidateToken(command.UserId);
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
                accessToken.ValidateToken(command.UserId, command.SubscriberId);
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

        [HttpGet("getall/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(string userId, int skip, int take)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetUsersQuery { Skip=skip, Take=take});
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

        [HttpGet("getuserverificationstatus/{userId}")]
        public async Task<ActionResult<Result>> GetUserVerificationStatus(string userId)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetUserVerificationStatusQuery {UserId = userId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Getting User Verification Status  was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getallusers/{subscriberId}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAllUsers(int subscriberId, string userId,int skip, int take)
        {
            try
            {
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetUsersQuery { SubscriberId = subscriberId, UserId= userId, Skip =skip, Take=take});
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

        [HttpGet("getbyid/{id}/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetById(string id, int subscriberId, string userId)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Please input valid user Id");
                }

                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { UserId = userId });
                if (subscriber.Entity == null)
                {
                    if (!accessToken.ValidateToken(userId))
                    {
                        throw new Exception("You're not authorized");
                    }

                }
                return await Mediator.Send(new GetUserByIdQuery { Id = id, SubscriberId = subscriberId, UserId = userId });
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

        [HttpGet("getbysubscriberId/{subscriberId}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetBySubscriberId(int subscriberId, string userId, int skip, int take)
        {
            try
            {

                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { Id = subscriberId });
                if (subscriber.Entity ==null)
                {
                    accessToken.ValidateToken(userId, subscriberId);
                }
               
                if (subscriberId <= 0)
                {
                    return BadRequest("Please input valid subscriber Id");
                }
                return await Mediator.Send(new GetUsersBySubscriberIdQuery { SubscriberId = subscriberId, Skip=skip, Take=take });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval by subscriber id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


     


        [HttpGet("getbyemail/{email}/{userId}")]
        public async Task<ActionResult<Result>> GetByEmail(string email, string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Please input valid email");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetUserByEmailQuery { Email = email });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "User retrieval by email was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
