
using Onyx.AuthServices.Application.Authentication.Commands.DeveloperLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Onyx.AuthService.API.Controllers;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Clients.Commands.CreateClient;
using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.RefreshToken;

namespace Onyx.AuthServices.API.Controllers
{
    public class ApplicationAuthController : ApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(ApplicationLoginCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return new AuthResult { IsSuccess = false, Message = ex?.Message ?? ex?.InnerException?.Message+""+ "Error:"+ ex.GetErrors()};
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Authentication login was not successful: " + ex?.Message + ex?.InnerException.Message };
               // return AuthResult.Failure( "Login was not successful" + ex?.Message ?? ex?.InnerException?.Message );
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthResult>> RefreshToken(GenerateRefreshTokenCommand command)
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
                return new AuthResult { IsSuccess = false, Message = " Authentication login by refresh token was not successful: " + ex?.Message + ex?.InnerException.Message };

            }
        }

        // [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateAppLogin(CreateClientCommand command)
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
               
                return Result.Failure(new string[]{"Logout was not successful: " + ex?.Message + ex?.InnerException.Message });
            }
            


        }

    }
}