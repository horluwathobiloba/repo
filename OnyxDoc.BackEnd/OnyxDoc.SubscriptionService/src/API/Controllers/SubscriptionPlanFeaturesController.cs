using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Commands;
using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubscriptionPlanFeaturesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public SubscriptionPlanFeaturesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSubscriptionPlanFeatureCommand command)
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
                return Result.Failure($"Subscription plan feature creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createsubscriptionplanfeatures")]
        public async Task<ActionResult<Result>> CreateSubscriptionPlanFeatures(CreateSubscriptionPlanFeaturesCommand command)
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
                return Result.Failure($"Subscription plan features creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateSubscriptionPlanFeatureCommand command)
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
                return Result.Failure($"Subscription plan feature update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatesubscriptionplanfeatures")]
        public async Task<ActionResult<Result>> UpdateSubscriptionPlanFeatures(UpdateSubscriptionPlanFeaturesCommand command)
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
                return Result.Failure($"Update subscription plan features failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatesubscriptionplanfeature")]
        public async Task<ActionResult<Result>> ActivateSubscriptionnPlanFeature(ActivateSubscriptionPlanFeatureCommand command)
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
                return Result.Failure($"Activate subscription plan feature failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatesubscriptionplanfeature")]
        public async Task<ActionResult<Result>> DeactivateSubscriptionPlanFeature(DeactivateSubscriptionPlanFeatureCommand command)
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
                return Result.Failure($"Deactivate subscription plan feature failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        // [HttpPost("updatesubscriptionplanfeaturestatus")]
        private async Task<ActionResult<Result>> UpdateSubscriptionPlanFeatureStatus(UpdateSubscriptionPlanFeatureStatusCommand command)
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
                return Result.Failure($"Subscription plan feature creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getsubscriptionplanfeaturebyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlanFeatureById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanFeatureByIdQuery { SubscriberId = subscriberid, Id = id, UserId =  userid, AccessToken = accessToken });
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

        [HttpGet("getsubscriptionplanfeaturesbysubscription/{subscriberid}/{subscriptionplanid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlanFeaturesBySubscription(int subscriberid, int subscriptionplanid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanFeaturesBySubscriptionQuery { SubscriberId = subscriberid, SubscriptionPlanId = subscriptionplanid, 
                     UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get subscription plan featuress by subscription failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getsubscriptionplanfeatures/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetSubscriptionPlanFeatures(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanFeaturesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all subscription plan features was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivesubscriptionplanfeatures/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveSubscriptionPlanFeatures(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveSubscriptionPlanFeaturesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active subscription plan features was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallsubscriptionplanfeatures/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllSubscriptionPlanFeatures(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetSubscriptionPlanFeaturesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving subscription plan features was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}