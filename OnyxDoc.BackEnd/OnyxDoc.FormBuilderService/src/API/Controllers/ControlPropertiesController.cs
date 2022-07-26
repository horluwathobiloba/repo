using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ControlPropertiesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ControlPropertiesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateControlPropertyCommand command)
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
                return Result.Failure($"Control property creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcontrolproperties")]
        public async Task<ActionResult<Result>> CreateControlProperties(CreateControlPropertiesCommand command)
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
                return Result.Failure($"Control properties creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateControlPropertyCommand command)
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
                return Result.Failure($"Control property creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontrolproperties")]
        public async Task<ActionResult<Result>> UpdateControlProperties(UpdateControlPropertiesCommand command)
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
                return Result.Failure($"Update control properties failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatecontrolproperty")]
        public async Task<ActionResult<Result>> ActivateSubscriptionnPricing(ActivateControlPropertyCommand command)
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
                return Result.Failure($"Activate control property failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatecontrolproperty")]
        public async Task<ActionResult<Result>> DeactivateControlProperty(DeactivateControlPropertyCommand command)
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
                return Result.Failure($"Deactivate control property failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deletecontrolproperty")]
        public async Task<ActionResult<Result>> DeleteControlProperty(DeleteControlPropertyCommand command)
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
                return Result.Failure($"Delete control property failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        // [HttpPost("updatecontrolpropertiestatus")]
        private async Task<ActionResult<Result>> UpdateControlPropertyStatus(UpdateControlPropertyStatusCommand command)
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
                return Result.Failure($"Control property creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontrolpropertybyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetControlPropertyById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertyByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control properties failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrolpropertiesbycontrol/{subscriberid}/{controlid}/{userid}")]
        public async Task<ActionResult<Result>> GetControlPropertiesBySubscription(int subscriberid, int controlid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertiesByControlQuery { SubscriberId = subscriberid, UserId = userid, ControlId = controlid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control properties by control failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }      

        [HttpGet("getcontrolproperties/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetControlProperties(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertiesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all control property was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivecontrolproperties/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveControlProperties(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveControlPropertiesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active control property was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getallcontrolproperties/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllControlProperties(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertiesQuery() { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving control property was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}