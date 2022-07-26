using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Subscriptions.Commands;
using RubyReloaded.SubscriptionService.Application.Common.Exceptions;
using RubyReloaded.SubscriptionService.Application.Subscriptions.Queries;
using RubyReloaded.SubscriptionService.Domain.Enums;

namespace RubyReloaded.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubscriptionsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public SubscriptionsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSubscriptionCommand command)
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
                return Result.Failure($"Subscription creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateSubscriptionCommand command)
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
                return Result.Failure($"Subscription update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatesubscription")]
        public async Task<ActionResult<Result>> ActivateSubscription(ActivateSubscriptionCommand command)
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
                return Result.Failure($"Subscription creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatesubscription")]
        public async Task<ActionResult<Result>> DeactivateSubscription(DeactivateSubscriptionCommand command)
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
                return Result.Failure($"Subscription creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        // [HttpPost("updatesubscriptionstatus")]
        private async Task<ActionResult<Result>> UpdateSubscriptionStatus(UpdateSubscriptionStatusCommand command)
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
                return Result.Failure($"Subscription creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getsubscriptionbyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription payment failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionsbyplan/{subscriberid}/{subscriptionplanid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionsByPlan(int subscriberid, int subscriptionplanid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionsByPlanQuery { SubscriberId = subscriberid, SubscriptionPlanId = subscriptionplanid, UserId = userid, 
                    AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscriptions by subscription failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getactivesubscriptionsbyplan/{subscriberid}/{subscriptionplanid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveSubscriptionsByPlan(int subscriberid, int subscriptionplanid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveSubscriptionsByPlanQuery { SubscriberId = subscriberid, SubscriptionPlanId = subscriptionplanid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscriptions by subscription failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getsubscriptions/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptions(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all subscription was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivesubscriptions/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveSubscriptions(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveSubscriptionsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active subscription was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallsubscriptions/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllSubscriptions(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionsQuery() { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving subscription was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}