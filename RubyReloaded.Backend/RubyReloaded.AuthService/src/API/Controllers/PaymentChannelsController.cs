using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.PaymentChannels.Commands.CreatePaymentChannel;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.PaymentChannels.Commands.UpdatePaymentChannel.UpdatePaymentChannelCommand;
using RubyReloaded.AuthService.Application.PaymentChannels.Queries.GetPaymentChannels;
using RubyReloaded.AuthService.Application.PaymentChannels.Commands.ChangePaymentChannel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;

namespace API.Controllers
{
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentChannelsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentChannelsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreatePaymentChannel(CreatePaymentChannelCommand command)
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
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment Channel creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPut("update/{id}")]
        public async Task<ActionResult<Result>> UpdatePaymentChannel(int id, [FromBody] UpdatePaymentChannelCommand command)
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
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment Channel update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {
                return await Mediator.Send(new GetPaymentChannelByIdQuery { Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Payment Channel by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAllPaymentChannels(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetPaymentChannelsQuery { 
                Skip=skip,
                Take=take
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all Payment Channels was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changepaymentchannelstatus")]
        public async Task<ActionResult<Result>> ChangePaymentChannelStatus(ChangePaymentChannelStatusCommand command)
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
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Changing Payment Channel status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
