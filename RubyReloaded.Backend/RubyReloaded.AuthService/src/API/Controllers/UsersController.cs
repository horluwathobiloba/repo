using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.User.Command.CreateUser;
using RubyReloaded.AuthService.Application.User.Queries.GetUser;
using RubyReloaded.AuthService.Application.User.Command.UpdateUser;
using RubyReloaded.AuthService.Application.User.Command.ChangeUserStatus;
using RubyReloaded.AuthService.Application.User.Command.GetInvitationLink;
using RubyReloaded.AuthService.Application.User.Command.VerifyInviteLink;
using RubyReloaded.AuthService.Application.Users.Command.VerifyEmail;
using RubyReloaded.AuthService.Application.Users.Command.VerifyOtp;
using RubyReloaded.AuthService.Application.Users.Command.CreateOTP;
using RubyReloaded.AuthService.Application.Users.Command.ChangeUserAccessStatus;

using RubyReloaded.AuthService.Application.Users.Command.SendInvitationCode;
using RubyReloaded.AuthService.Application.Users.Command.CreateCooperativeUserMapping;
using RubyReloaded.AuthService.Application.Users.Command.UpdateTransactionPin;
using RubyReloaded.AuthService.Application.Users.Command.UpdateBVN;
using RubyReloaded.AuthService.Application.Users.Command.VerifyUserTransactionPin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;
using RubyReloaded.AuthService.Application.Users.Command.UpdatePhoneNumber;
using RubyReloaded.AuthService.Application.Users.Queries.GetUser;
using RubyReloaded.AuthService.Application.Users.Command.UserSignUp;
using RubyReloaded.AuthService.Application.Users.Command.ChangeUserNotificationStatus;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController: ApiController
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
        [HttpPost("usersignup")]
        public async Task<ActionResult<Result>> UserSignUp(UserSignUpCommand command)
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


        [HttpPost("createusercooperative")]
        public async Task<ActionResult<Result>> Create(CreateUserCooperativeCommand command)
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



        [HttpPost("createcooperativeuser")]
        public async Task<ActionResult<Result>> CreateCooperativeUser(CreateCooperativeUserMappingCommand command)
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
        [HttpPost("createLink")]
        public async Task<ActionResult<Result>> CreateLink(CreateInvitationLinkCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
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



        [HttpPost("updatetransactionpin")]
        public async Task<ActionResult<Result>> UpdateTransactionPin(UpdateTransactionPinCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Update Transaction Pin was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("updatebvn")]
        public async Task<ActionResult<Result>> UpdateBVN(UpdateBVNCommand command)
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

                return Result.Failure(new string[] { "Update BVN was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("verifytransactionpin")]
        public async Task<ActionResult<Result>> VerifyTransactionPin(VerifyTransactionPinCommand command)
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

                return Result.Failure(new string[] { "Verification was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("sendinvitationcode")]
        public async Task<ActionResult<Result>> SendInvitationCode(SendInvitationCodeCommand command)
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



        [HttpPost("createotp")]
        public async Task<ActionResult<Result>> CreateOtp(CreateOTPCommand command)
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

        [HttpPost("verifylink")]
        public async Task<ActionResult<Result>> VerifyLink(VerifyInviteLinkCommand command)
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


        [HttpPost("verifyotp")]
        public async Task<ActionResult<Result>> VerifyOtp(VerifyOtpCommand command)
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

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateUserCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.LoggedInUserId))
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
        [HttpPost("changeuseraccessstatus")]
        public async Task<ActionResult<Result>> ChangeUserAccessStatus(ChangeUserAccessStatusCommand command)
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

                return Result.Failure(new string[] { "Changing User Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("emailconfirmation")]
        public async Task<ActionResult<Result>> EmailConfirmation(VerifyEmailCommand command)
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
       
           [HttpPost("updatephonenumber")]
        public async Task<ActionResult<Result>> UpdatePhoneNumber(UpdatePhoneNumberCommand command)
        {
            try
            {
                if(string.IsNullOrEmpty(command.Email))
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
        [HttpPost("changenotificationstatus")]
        public async Task<ActionResult<Result>> ChangeNotificationStatus(ChangeNotificationStatusCommand command)
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
                return Result.Failure(new string[] { "Sending Email Confirmation was unsuccessful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAll(int skip, int take,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAllUsersQuery { 
                Skip=skip,
                Take=take
                });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
       
        [HttpGet("getbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetById(string id,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
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
      
        [HttpGet("getusertokenbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetUserTokenById(string id, string userid)
        {
            try
            {
                //accessToken.ValidateToken(userid);
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Please input valid user Id");
                }
                return await Mediator.Send(new GetUserTokenById { Id = id });
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



        [HttpGet("getbycooperative/{cooperativeid}/{userid}")]
        public async Task<ActionResult<Result>> GetByCooperativeId(int cooperativeId,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetUsersByCooperativeIdQuery { CooperativeId = cooperativeId });
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
        [HttpGet("getdashboard")]
        public async Task<ActionResult<Result>> GetDashBoard()
        {
            try
            {
               
                return await Mediator.Send(new GetUserDashBoardDetails());
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
    }

    
}
