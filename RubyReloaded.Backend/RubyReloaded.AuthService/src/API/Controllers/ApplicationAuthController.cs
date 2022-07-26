
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Clients.Commands.CreateClient;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Authentication;

namespace RubyReloaded.AuthServices.API.Controllers
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
                return new AuthResult { IsSuccess = false, Message = " Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };
               // return AuthResult.Failure( "Login was not successful" + ex?.Message ?? ex?.InnerException?.Message );
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
               
                return Result.Failure(new string[]{"Logout was not successful: " + ex?.Message ?? ex?.InnerException.Message });
            }
            


        }

    }
}