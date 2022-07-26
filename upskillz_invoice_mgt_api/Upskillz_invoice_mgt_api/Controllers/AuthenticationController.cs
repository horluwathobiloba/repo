using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Application.Authentication.AdminLogin;
using Upskillz_invoice_mgt_Application.Authentication.AdminLogin.LoginDto;
using Upskillz_invoice_mgt_Application.Authentication.AdminSignUp;
using Upskillz_invoice_mgt_Application.Common;
using Upskillz_invoice_mgt_Application.Common.Exceptions;
using Upskillz_invoice_mgt_Infrastructure.Policy;

namespace Upskillz_invoice_mgt_api.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost("Admin-login")]
        //[Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult<Response<LoginResponseDto>>> Login(AdminLoginCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return new Response<LoginResponseDto> { Succeeded = false, Message = ex?.Message ?? ex?.InnerException?.Message + "" + "Error:" + ex.GetErrors() };
            }
            catch (Exception ex)
            {
                return new Response<LoginResponseDto> { Succeeded = false, Message = " Authentication login was not successful: " + ex?.Message ?? ex?.InnerException.Message };
            }
        }


        [HttpPost("Admin-signup")]
        //[Authorize(Policy = Policies.SuperAdmin)]
        public async Task<ActionResult<Response<string>>> AdminSignUp(AdminSignUpCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return new Response<string> { Succeeded = false, Message = ex?.Message ?? ex?.InnerException?.Message + "" + "Error:" + ex.GetErrors() };
            }
            catch (Exception ex)
            {
                return new Response<string> { Succeeded = false, Message = "Sign up was not successful: " + ex?.Message ?? ex?.InnerException.Message };
            }
        }
    }
}
