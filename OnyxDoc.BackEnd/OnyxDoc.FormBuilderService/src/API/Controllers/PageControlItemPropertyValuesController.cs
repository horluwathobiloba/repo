using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PageControlItemPropertyValuesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PageControlItemPropertyValuesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreatePageControlItemPropertyValueCommand command)
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
                return Result.Failure($"page control item property value creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createpagecontrolitempropertyvalues")]
        public async Task<ActionResult<Result>> CreatePageControlItemPropertyValues(CreatePageControlItemPropertyValuesCommand command)
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
                return Result.Failure($"page control item property values creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdatePageControlItemPropertyValueCommand command)
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
                return Result.Failure($"page control item property value creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatepagecontrolitempropertyvalues")]
        public async Task<ActionResult<Result>> UpdatePageControlItemPropertyValues(UpdatePageControlItemPropertyValuesCommand command)
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
                return Result.Failure($"Update page control item properties failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatepagecontrolitempropertyvalue")]
        public async Task<ActionResult<Result>> ActivatePageControlItemPropertyValue(ActivatePageControlItemPropertyValueCommand command)
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

        [HttpPost("deactivatepagecontrolitempropertyvalue")]
        public async Task<ActionResult<Result>> DeactivatePageControlItemPropertyValue(DeactivatePageControlItemPropertyValueCommand command)
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

        [HttpPost("deletepagecontrolitempropertyvalue")]
        public async Task<ActionResult<Result>> DeletePageControlItemPropertyValue(DeletePageControlItemPropertyValueCommand command)
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

        //// [HttpPost("updatepagecontrolitempropertyvaluestatus")]
        //private async Task<ActionResult<Result>> UpdatePageControlItemPropertyValueStatus(UpdatePageControlItemPropertyValueStatusCommand command)
        //{
        //    try
        //    {
        //        command.AccessToken = accessToken;
        //        return await Mediator.Send(command);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Result.Failure($"page control item property value creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
        //    }
        //}

        [HttpGet("getpagecontrolitempropertyvaluebyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemPropertyValueById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertyValueByIdQuery
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

        [HttpGet("getpagecontrolitempropertyvaluesbypagecontrolitemproperty/{subscriberid}/{pagecontrolitempropertyid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemPropertyValuesByPageControlItemProperty(int subscriberid, int pagecontrolitempropertyid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertyValuesByPCIPropertyQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    PageControlItemPropertyId = pagecontrolitempropertyid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page control item properties by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpagecontrolitempropertyvalues/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemPropertyValues(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertyValuesQuery
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

        [HttpGet("getactivepagecontrolitempropertyvalues/{subscriberid}/{pagecontrolitempropertyid}/{userid}")]
        public async Task<ActionResult<Result>> GetActivePageControlItemPropertyValues(int subscriberid, int pagecontrolitempropertyid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActivePageControlItemPropertyValuesQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    PageControlItemPropertyId = pagecontrolitempropertyid,
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

        [HttpGet("getallpagecontrolitempropertyvalues/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllPageControlItemPropertyValues(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemPropertyValuesQuery()
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