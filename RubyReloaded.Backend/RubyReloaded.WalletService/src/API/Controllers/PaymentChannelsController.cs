

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.PaymentChannels.Commands.CreatePaymentChannel;
using RubyReloaded.WalletService.Infrastructure.Utility;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Net;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.PaymentChannels.Commands.UpdatePaymentChannel.UpdatePaymentChannelCommand;
using RubyReloaded.WalletService.Application.PaymentChannels.Queries.GetPaymentChannels;
using RubyReloaded.WalletService.Application.PaymentChannels.Commands.ChangePaymentChannel;
using RubyReloaded.WalletService.Domain.Enums;

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
                accessToken.ValidateToken(command.UserId);
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
                accessToken.ValidateToken(command.UserId);
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
                return await Mediator.Send(new GetPaymentChannelsQuery
                {
                    Skip = skip,
                    Take = take
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


        [HttpGet("getpaymentchannelsbytype/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetPaymentChannelsByType(int skip, int take, PaymentChannelType paymentChannelType, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetPaymentChannelsByType
                {
                    Skip = skip,
                    Take = take
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Payment Channels By Type was not successful" + ex?.Message ?? ex?.InnerException?.Message });
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
