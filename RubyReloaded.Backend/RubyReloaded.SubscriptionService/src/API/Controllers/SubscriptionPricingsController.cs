using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands;
using RubyReloaded.SubscriptionService.Application.Common.Exceptions;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;

namespace RubyReloaded.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubscriptionPlanPricingsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public SubscriptionPlanPricingsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSubscriptionPlanPricingCommand command)
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
                return Result.Failure($"Subscription pricing creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createsubscriptionpricings")]
        public async Task<ActionResult<Result>> CreateSubscriptionPlanPricings(CreateSubscriptionPlanPricingsCommand command)
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
                return Result.Failure($"Subscription pricings creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateSubscriptionPlanPricingCommand command)
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
                return Result.Failure($"Subscription pricing creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatesubscriptionpricings")]
        public async Task<ActionResult<Result>> UpdateSubscriptionPlanPricings(UpdateSubscriptionPlanPricingsCommand command)
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
                return Result.Failure($"Update subscription pricings failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatesubscriptionpricing")]
        public async Task<ActionResult<Result>> ActivateSubscriptionnPricing(ActivateSubscriptionPlanPricingCommand command)
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
                return Result.Failure($"Activate subscription currency failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatesubscriptionpricing")]
        public async Task<ActionResult<Result>> DeactivateSubscriptionPlanPricing(DeactivateSubscriptionPlanPricingCommand command)
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
                return Result.Failure($"Deactivate subscription currency failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

       // [HttpPost("updatesubscriptionpricingstatus")]
        private async Task<ActionResult<Result>> UpdateSubscriptionPlanPricingStatus(UpdateSubscriptionPlanPricingStatusCommand command)
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
                return Result.Failure($"Subscription pricing creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getsubscriptionpricingbyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlanPricingById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanPricingByIdQuery { SubscriberId = subscriberid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription plan feature failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionpricingsbysubscriptionplan/{subscriberid}/{subscriptionplanid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlanPricingsBySubscription(int subscriberid, int subscriptionplanid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanPricingsBySubscriptionPlanQuery { SubscriberId = subscriberid, SubscriptionPlanId = subscriptionplanid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription pricings by subscription failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
      

        [HttpGet("getsubscriptionpricings/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlanPricings(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanPricingsQuery { SubscriberId = subscriberid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all subscription pricing was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivesubscriptionpricings/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveSubscriptionPlanPricings(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveSubscriptionPlanPricingsQuery { SubscriberId = subscriberid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active subscription pricing was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallsubscriptionpricings/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllSubscriptionPlanPricings(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanPricingsQuery() { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving subscription pricing was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}