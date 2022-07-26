using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PageControlItemsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PageControlItemsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreatePageControlItemCommand command)
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

        [HttpPost("createpagecontrolitems")]
        public async Task<ActionResult<Result>> CreatePageControlItems(CreatePageControlItemsCommand command)
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
        public async Task<ActionResult<Result>> Update(UpdatePageControlItemCommand command)
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

        [HttpPost("updatepagecontrolitems")]
        public async Task<ActionResult<Result>> UpdatePageControlItems(UpdatePageControlItemsCommand command)
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
                return Result.Failure($"Update page control items failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatepagecontrolitem")]
        public async Task<ActionResult<Result>> ActivatePageControlItem(ActivatePageControlItemCommand command)
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
                return Result.Failure($"Activate page control item failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatepagecontrolitem")]
        public async Task<ActionResult<Result>> DeactivatePageControlItem(DeactivatePageControlItemCommand command)
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
                return Result.Failure($"Deactivate page control item failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deletepagecontrolitem")]
        public async Task<ActionResult<Result>> DeletePageControlItem(DeletePageControlItemCommand command)
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
                return Result.Failure($"Delete page control item failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        // [HttpPost("updatepagecontrolitemstatus")]
        private async Task<ActionResult<Result>> UpdatePageControlItemStatus(UpdatePageControlItemStatusCommand command)
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

        [HttpGet("getpagecontrolitembyid/{subscriberid}/{id}/{documentid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemById(int subscriberid, int id, int documentid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, DocumentId = documentid, AccessToken = accessToken });
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

        [HttpGet("getpagecontrolitem/{subscriberid}/{userid}/{documentpageid}/{controlid}")]
        public async Task<ActionResult<Result>> GetPageControlItem(int subscriberid, int documentpageid, int controlid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    DocumentPageId = documentpageid,
                    ControlId = controlid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page control items by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpagecontrolitemsbydocument/{subscriberid}/{documentid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemsByDocument(int subscriberid, int documentid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemsByDocumentQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    DocumentId = documentid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page control items by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getpagecontrolitemsbypage/{subscriberid}/{documentpageid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItemsByPage(int subscriberid, int documentpageid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemsByPageQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    DocumentPageId = documentpageid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get page control items by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getpagecontrolitems/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetPageControlItems(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all page control item was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivepagecontrolitems/{subscriberid}/{documentpageid}/{userid}")]
        public async Task<ActionResult<Result>> GetActivePageControlItems(int subscriberid, int documentpageid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActivePageControlItemsQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    DocumentPageId = documentpageid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active page control item was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getallpagecontrolitems/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllPageControlItems(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetPageControlItemsQuery() { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving page control item was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}