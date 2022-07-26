using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using OnyxDoc.AuthService.Application.Authentication.ForgotPassword;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Authentication.Commands.LogOut;
using OnyxDoc.AuthService.Application.Authentication.Commands.ChangePassword;
using OnyxDoc.AuthService.Application.Authentication.ResetPassword;
using OnyxDoc.AuthService.Application.Authentication.Commands.Login;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using OnyxDoc.AuthService.Infrastructure.Utility;

namespace OnyxDoc.AuthService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return new AuthResult { IsSuccess = false, Message = ex?.Message ?? ex?.InnerException?.Message + "" + "Error:" + ex.GetErrors() };
            }
            catch (System.Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };
               
            }
        }

        [HttpPost("loginwiththirdparty")]
        public async Task<ActionResult<AuthResult>> LoginWithThirdParty(LoginWithThirdPartyCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return new AuthResult { IsSuccess = false, Message = ex?.Message ?? ex?.InnerException?.Message + "" + "Error:" + ex.GetErrors() };
            }
            catch (System.Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };

            }
        }

        [HttpPost("loginwithlinkedin")]
        public async Task<ActionResult<AuthResult>> LoginWithLinkedIn(LinkedInSignInCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return new AuthResult { IsSuccess = false, Message = ex?.Message ?? ex?.InnerException?.Message + "" + "Error:" + ex.GetErrors() };
            }
            catch (System.Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " LinkedIn login was not successful: " + ex?.Message ?? ex?.InnerException.Message };

            }
        }



        [HttpPost("logout")]
        public async Task<ActionResult<Result>> LogOut( LogoutCommand command)
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

                return Result.Failure(new string[] { "Authentication logout was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changepassword")]
        public async Task<ActionResult<Result>> ChangePassword(ChangePasswordCommand command)
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
                return Result.Failure(new string[] { "Change Password update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        } 
        [HttpPost("forgotpassword")]
        public async Task<ActionResult<Result>> ForgotPassword(ForgotPasswordCommand command)
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
                return Result.Failure(new string[] { "Forgot Password was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }
        [HttpPost("resetpassword")]
        public async Task<ActionResult<Result>> ResetPassword(ResetPasswordCommand command)
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
                return Result.Failure(new string[] { "Reset Password Completion was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("linkedinsignup")]
        public async Task<ActionResult<Result>> LinkedInSignUp()
        {
            try
            {
                return await Mediator.Send(new LinkedInSignUpCommand());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "LinkedIn Sign Up was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("linkedinauthorization")]
        public async Task<ActionResult<Result>> LinkedInAuthorization(LinkedInAuthorizationCommand command)
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
                return Result.Failure(new string[] { "LinkedIn Authorization was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
