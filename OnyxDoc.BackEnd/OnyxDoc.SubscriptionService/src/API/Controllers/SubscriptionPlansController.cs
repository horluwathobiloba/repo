using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Commands;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubscriptionPlansController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public SubscriptionPlansController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSubscriptionPlanCommand command)
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
                return Result.Failure($"Subscription plan creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateSubscriptionPlanCommand command)
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
                return Result.Failure($"Subscription plan update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatesubscriptionplan")]
        public async Task<ActionResult<Result>> ActivateSubscription(ActivateSubscriptionPlanCommand command)
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
                return Result.Failure($"Activate subscription plan failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatesubscriptionplan")]
        public async Task<ActionResult<Result>> DeactivateSubscription(DeactivateSubscriptionPlanCommand command)
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
                return Result.Failure($"Deactivate subscription plan failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        //[HttpPost("updatesubscriptionstatus")]
        private async Task<ActionResult<Result>> UpdateSubscriptionPlanStatus(UpdateSubscriptionPlanStatusCommand command)
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
                return Result.Failure($"Update subscription plan status failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getsubscriptionplanbyid/{subscriberid}/{id}/{userid}")]
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
                return Result.Failure(new string[] { "Get subscription plan failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionplansbytype/{subscriberid}/{subscriptiontype}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlansByTypeQuery(int subscriberid, SubscriptionType subscriptiontype, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlansByTypeQuery
                {
                    SubscriberId = subscriberid,
                    SubscriptionType = subscriptiontype,
                    UserId = userid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription plans by type failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsubscriptionplans/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlans(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlansQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all subscription plans was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivesubscriptionplans/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveSubscriptionPlans(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveSubscriptionPlansQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active subscription plans was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getprevioussubscriptionplan/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetPreviousSubscriptionPlan(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPreviousSubscriptionPlanQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving previous subscription plan was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}