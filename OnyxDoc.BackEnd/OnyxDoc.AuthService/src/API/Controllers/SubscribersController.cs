using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.AspNetCore.Http; 
using OnyxDoc.AuthService.Application.Common.Exceptions; 
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Infrastructure.Utility; 
using OnyxDoc.AuthService.Application.Subscribers.Commands.CreateSubscriber;
using OnyxDoc.AuthService.Application.Subscribers.Commands.UpdateSubscriber;
using OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers;
using OnyxDoc.AuthService.Application.Commands.SignUp;

namespace OnyxDoc.AuthService.API.Controllers
{
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   
    public class SubscribersController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public SubscribersController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSubscriberCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Subscriber creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<Result>> SignUp(SignUpCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "SignUp was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("completesubscriberfreetrial")]
        public async Task<ActionResult<Result>> CompleteSubscriberFreeTrial(CompleteSubscriberFreeTrialCommand command)
        {
            try
            {
                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { UserId = command.UserId });
                if (subscriber.Entity == null)
                {
                    throw new Exception("You're not authorized");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Complete Subscriber Free Trial was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("activatesubscriberfreetrial")]
        public async Task<ActionResult<Result>> ActivateSubscriberFreeTrial(ActivateSubscriberFreeTrialCommand command)
        {
            try
            {
                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { UserId = command.UserId });
                if (subscriber.Entity == null)
                {
                    throw new Exception("You're not authorized");
                }
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Activate Subscriber Free Trial was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getallsubscribers/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(string userId, int skip, int take)
        {
            try
            {
                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { UserId = userId });
                if (subscriber.Entity == null)
                {
                    throw new Exception("You're not authorized");
                }
                return await Mediator.Send(new GetSubscribersQuery { Skip = skip, Take = take });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update/{userId}")]
        public async Task<ActionResult<Result>> Update(string userId, UpdateSubscriberCommand command)
        {
            try
            {
                //var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { Id = command.SubscriberId,UserId = userId });
                //if (subscriber.Entity == null)
                //{
                //    throw new Exception("You're not authorized");
                //}
                accessToken.ValidateToken(command.UserId, command.SubscriberId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Subscriber update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id, string userId)
        {
            try
            {
                var subscriber = await Mediator.Send(new GetAdminSubscriberQuery { Id = id, UserId =userId});
                if (subscriber.Entity == null)
                {
                    if (!accessToken.ValidateToken(userId))
                    {
                        throw new Exception("You're not authorized");
                    }
                }
                
                return await Mediator.Send(new GetSubscriberByIdQuery { Id = id ,UserId = userId});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Subscriber retrieval by id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
