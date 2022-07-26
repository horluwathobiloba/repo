using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.PaymentChannels.Commands;
using RubyReloaded.SubscriptionService.Application.Common.Exceptions;
using RubyReloaded.SubscriptionService.Application.PaymentChannels.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;

namespace RubyReloaded.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentChannelsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentChannelsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreatePaymentChannelCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Payment channel creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createpaymentchannels")]
        public async Task<ActionResult<Result>> CreatePaymentChannels(CreatePaymentChannelsCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Payment channel creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdatePaymentChannelCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Payment channel update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatepaymentchannels")]
        public async Task<ActionResult<Result>> UpdatePaymentChannels(UpdatePaymentChannelsCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Payment channels update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }


        [HttpPost("updatepaymentchannelstatus")]
        public async Task<ActionResult<Result>> UpdatePaymentChannelStatus(int id, int subscriberId, string userId)
        {
            try
            {
                var command = new UpdatePaymentChannelStatusCommand { Id = id, SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken, Status = Status.Active }; 
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Payment channel creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatepaymentchannel")]
        public async Task<ActionResult<Result>> ActivatePaymentChannel(ActivatePaymentChannelCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Activate payment channel failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatepaymentchannel")]
        public async Task<ActionResult<Result>> DeactivatePaymentChannel(DeactivatePaymentChannelCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Deactivate payment channel failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

       // [HttpPost("updatepaymentchannelstatus")]
        private async Task<ActionResult<Result>> UpdatePaymentChannelStatus(UpdatePaymentChannelStatusCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Payment channel creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getpaymentchannelbyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetPaymentChannelById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPaymentChannelByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get paymanet channel failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpaymentchannels/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetPaymentChannels(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPaymentChannelsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all payment channels was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpaymentchannelsbycurrency/{subscriberid}/{currencycode}/{userid}")]
        public async Task<ActionResult<Result>> GetPaymentChannelsByCurrency(int subscriberid, string currencycode, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPaymentChannelsByCurrencyQuery { SubscriberId = subscriberid, CurrencyCode = currencycode, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get payment channel by currency code was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivepaymentchannels/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActivePaymentChannels(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActivePaymentChannelsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active payment channels was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}