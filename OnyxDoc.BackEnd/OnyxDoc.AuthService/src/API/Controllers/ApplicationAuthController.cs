
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OnyxDoc.AuthService.API.Controllers;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Clients.Commands.CreateClient;
using OnyxDoc.AuthService.Application.Authentication;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OnyxDoc.AuthServices.API.Controllers
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
                return new AuthResult { IsSuccess = false, Message = ex?.Message ?? ex?.InnerException?.Message + "" + "Error:" + ex.GetErrors() };
            }
            catch (Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };
            }
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

                return Result.Failure(new string[] { "Logout was not successful: " + ex?.Message ?? ex?.InnerException.Message });
            }

        }

    }
}
