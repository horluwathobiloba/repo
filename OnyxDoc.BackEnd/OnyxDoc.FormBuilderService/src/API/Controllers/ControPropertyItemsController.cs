using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ControlPropertyItemsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ControlPropertyItemsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateControlPropertyItemCommand command)
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
                return Result.Failure($"Control property item creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcontrolpropertyitems")]
        public async Task<ActionResult<Result>> CreateControlPropertyItems(CreateControlPropertyItemsCommand command)
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
                return Result.Failure($"Control property items creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateControlPropertyItemCommand command)
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
                return Result.Failure($"Control property item creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontrolpropertyitems")]
        public async Task<ActionResult<Result>> UpdateControlPropertyItems(UpdateControlPropertyItemsCommand command)
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
                return Result.Failure($"Update control property items failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatecontrolpropertyitem")]
        public async Task<ActionResult<Result>> ActivateControlPropertyItem(ActivateControlPropertyItemCommand command)
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
                return Result.Failure($"Activate control property item failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatecontrolpropertyitem")]
        public async Task<ActionResult<Result>> DeactivateControlPropertyItem(DeactivateControlPropertyItemCommand command)
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
                return Result.Failure($"Deactivate control property item failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deletecontrolpropertyitem")]
        public async Task<ActionResult<Result>> DeleteControlPropertyItem(DeleteControlPropertyItemCommand command)
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
                return Result.Failure($"Delete control property item failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        // [HttpPost("updatecontrolpropertyitemstatus")]
        private async Task<ActionResult<Result>> UpdateControlPropertyItemStatus(UpdateControlPropertyItemStatusCommand command)
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
                return Result.Failure($"Control property item creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontrolpropertyitembyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetControlPropertyItemById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertyItemByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control property item failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrolpropertyitemsbycontrolproperty/{subscriberid}/{controlpropertyid}/{userid}")]
        public async Task<ActionResult<Result>> GetControlPropertyItemsByControlProperty(int subscriberid, int controlpropertyid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertyItemsByControlPropertyQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    ControlPropertyId = controlpropertyid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control property items by control property failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getcontrolpropertyitems/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetControlPropertyItems(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertyItemsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all control property item was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivecontrolpropertyitems/{controlpropertyid}/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveControlPropertyItems(int controlpropertyid, int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveControlPropertyItemsQuery
                {
                    SubscriberId = subscriberid,
                    ControlPropertyId = controlpropertyid,
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
                return Result.Failure(new string[] { "Retrieving active control property item was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallcontrolpropertyitems/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllControlPropertyItems(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlPropertyItemsQuery() { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving control property item was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}