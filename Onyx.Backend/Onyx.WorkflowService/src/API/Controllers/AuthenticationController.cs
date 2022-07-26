using Onyx.WorkFlowService.Application.Organizations.Commands.CreateOrganization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Application.Organizations.Commands.UpdateOrganization;
using Onyx.WorkFlowService.Application.Authentication.Commands.Login;
using Onyx.WorkFlowService.Application.Authentication.Commands.LogOut;
using Onyx.WorkFlowService.Application.Common.Models;
using Onyx.WorkFlowService.Application.Authentication.Commands.ChangePassword;
using Onyx.WorkFlowService.Application.Authentication.Commands.GeolocationLogin;

namespace Onyx.WorkFlowService.API.Controllers
{

    public class AuthenticationController : ApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = " Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };
               
            }
        }

        [HttpPost("mobilelogin")]
        public async Task<ActionResult<AuthResult>> MobileLogin(GeolocationLoginCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (System.Exception ex)
            {
                return new AuthResult { IsSuccess = false, Message = "Mobile Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };

            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult<Result>> LogOut( LogoutCommand command)
        {

            try
            {
                return await Mediator.Send(command);
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
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Authentication update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }
    }
}
