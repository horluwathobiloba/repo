using OnyxDoc.AuthService.Application.Features.Commands.CreateFeature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using OnyxDoc.AuthService.Infrastructure.Utility;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Features.Queries.GetFeatures;
using OnyxDoc.AuthService.Application.Features.Commands.ChangeFeature;
using OnyxDoc.AuthService.Application.Features.Commands.UpdateFeature;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers;

namespace OnyxDoc.AuthService.API.Controllers
{
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   
    public class FeaturesController : ApiController       
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public FeaturesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateFeatureCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId,command.SubscriberId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Feature creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

   
        [HttpPost("update/{featureId}")]
        public async Task<ActionResult<Result>> Update(int featureId, UpdateFeatureCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                if (featureId != command.FeatureId || (featureId == 0 || command.FeatureId == 0))
                {
                    return BadRequest("Invalid Feature Id");
                }
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Feature update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }

    

        [HttpPost("changefeaturestatus")]
        public async Task<ActionResult<Result>> ChangeFeatureStatus(ChangeFeatureStatusCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId,command.SubscriberId);
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Changing Feature status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}/{subscriberId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(string userId, int subscriberId,int skip, int take)
        {
            try
            {
                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { Id = subscriberId });
                if (subscriber.Entity == null)
                {
                    return BadRequest("Unauthorized Subscriber");
                }
                accessToken.ValidateToken(userId,subscriberId);
                return await Mediator.Send(new GetFeaturesQuery { Skip=skip, Take=take});
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving  Features was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id,string userId)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Please input valid feature Id");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetFeatureByIdQuery { Id = id });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Feature by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


   
        [HttpGet("getbyname/{name}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetByName(string name,string userId, int skip, int take)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Please input valid feature name");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetFeatureByNameQuery { Name = name, Skip=skip, Take=take });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Feature by name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyaccesslevel/{accessLevel}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetFeaturePermissions(AccessLevel accessLevel, string userId, int skip, int take)
        {
            try
            {
                if (accessLevel <= 0)
                {
                    return BadRequest("Please input valid access level");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetFeaturesByAccessLevelQuery { AccessLevel = accessLevel, Skip=skip, Take=take });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving all Features by access level was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
