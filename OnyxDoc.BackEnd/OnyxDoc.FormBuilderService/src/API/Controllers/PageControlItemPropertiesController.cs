using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PageControlItemPropertiesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PageControlItemPropertiesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreatePageControlItemPropertyCommand command)
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
                return Result.Failure($"Page control item creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createpagecontrolitemproperties")]
        public async Task<ActionResult<Result>> CreatePageControlItemProperties(CreatePageControlItemPropertiesCommand command)
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
                return Result.Failure($"Page control items creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdatePageControlItemPropertyCommand command)
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
                return Result.Failure($"Page control item creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatepagecontrolitemproperties")]
        public async Task<ActionResult<Result>> UpdatePageControlItemProperties(UpdatePageControlItemPropertiesCommand command)
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
                return Result.Failure($"Update page control item propertys failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatepagecontrolitemproperty")]
        public async Task<ActionResult<Result>> ActivatePageControlItemProperty(ActivatePageControlItemPropertyCommand command)
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
                return Result.Failure($"Activate page control item property failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatepagecontrolitemproperty")]
        public async Task<ActionResult<Result>> DeactivatePageControlItemProperty(DeactivatePageControlItemPropertyCommand command)
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
                return Result.Failure($"Deactivate page control item property failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deletepagecontrolitemproperty")]
        public async Task<ActionResult<Result>> DeletePageControlItemProperty(DeletePageControlItemPropertyCommand command)
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
                return Result.Failure($"Delete page control item property failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        // [HttpPost("updatepagecontrolitempropertiestatus")]
        private async Task<ActionResult<Result>> UpdatePageControlItemPropertyStatus(UpdatePageControlItemPropertyStatusCommand command)
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
                return Result.Failure($"Page control item creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getpagecontrolitempropertybyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemPropertyById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertyByIdQuery
                {
                    SubscriberId = subscriberid,
                    Id = id,
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
                return Result.Failure(new string[] { "Get document plan feature failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpagecontrolitemproperty/{subscriberid}/{userid}/{pagecontrolitemid}/{controlpropertyid}")]
        public async Task<ActionResult<Result>> GetPageControlItemProperty(int subscriberid, int pagecontrolitemid, int controlpropertyid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertyQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    PageControlItemId = pagecontrolitemid,
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
                return Result.Failure(new string[] { "Get page control item propertys by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpagecontrolitempropertiesbypagecontrolitem/{subscriberid}/{pagecontrolitemid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemPropertiesByPageControlItem(int subscriberid, int pagecontrolitemid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertiesByPageControlItemQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    PageControlItemId = pagecontrolitemid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page control item propertys by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpagecontrolitemproperties/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemProperties(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertiesQuery
                {
                    SubscriberId = subscriberid,
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
                return Result.Failure(new string[] { "Retrieving all page control item properties was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivepagecontrolitemproperties/{subscriberid}/{documentpageid}/{userid}")]
        public async Task<ActionResult<Result>> GetActivePageControlItemProperties(int subscriberid, int pagecontrolitemid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActivePageControlItemPropertiesQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    PageControlItemId = pagecontrolitemid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active page control item property was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallpagecontrolitemproperties/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllPageControlItemProperties(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertiesQuery()
                {
                    SubscriberId = subscriberid,
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
                return Result.Failure(new string[] { "Retrieving page control item property was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}