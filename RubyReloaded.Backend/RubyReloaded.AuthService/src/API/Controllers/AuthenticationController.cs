using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Authentication.Commands.Login;
using RubyReloaded.AuthService.Application.Authentication.Commands.LogOut;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Authentication.Commands.ChangePassword;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.AuthService.Application.Authentication.ResetPassword;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System;
using RubyReloaded.AuthService.Application.Authentication;

namespace RubyReloaded.AuthService.API.Controllers
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationController : ApiController
    {
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
        
    }
}
