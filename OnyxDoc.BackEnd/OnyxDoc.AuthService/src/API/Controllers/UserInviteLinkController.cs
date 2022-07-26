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
using OnyxDoc.AuthService.Application.Subscribers.Commands.CreateSubscriber;
using OnyxDoc.AuthService.Application.Subscribers.Commands.UpdateSubscriber;
using OnyxDoc.AuthService.Application.GenerateUserInviteLink.Commands;
using OnyxDoc.AuthService.Application.GenerateInviteLink.Commands;

namespace OnyxDoc.AuthService.API.Controllers
{
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   
    public class UserInviteLinkController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public UserInviteLinkController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("generatelink")]
        public async Task<ActionResult<Result>> GenerateLink(GenerateUserInviteLinkCommand command)
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

                return Result.Failure(new string[] { "Link generation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("copylink")]
        public async Task<ActionResult<Result>> CopyLink(GenerateUserInviteLinkCommand command)
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

                return Result.Failure(new string[] { "Link generation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("sendinvitetomultipleusers")]
        public async Task<ActionResult<Result>> SendInviteToMultipleUsers(SendInviteToMultipleUsersCommand command)
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

                return Result.Failure(new string[] { "Link generation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpPost("confirmlink")]
        public async Task<ActionResult<Result>> ConfirmLink(ConfirmInviteLinkCommand command)
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

                return Result.Failure(new string[] { "Link confirmation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
