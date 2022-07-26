using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.DocumentPages.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentPagesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public DocumentPagesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateDocumentPageCommand command)
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
                return Result.Failure($"Document page creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createdocumentpages")]
        public async Task<ActionResult<Result>> CreateDocumentPages(CreateDocumentPagesCommand command)
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
                return Result.Failure($"Document pages creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateDocumentPageCommand command)
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
                return Result.Failure($"Document page update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatedocumentpages")]
        public async Task<ActionResult<Result>> UpdateDocumentPages(UpdateDocumentPagesCommand command)
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
                return Result.Failure($"Update document pages failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }


        [HttpPost("updatedocumentpagedimension")]
        public async Task<ActionResult<Result>> UpdateDocumentPageDimension(UpdateDocumentPageDimensionCommand command)
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
                return Result.Failure($"Document page dimension update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatedocumentpagesdimensions")]
        public async Task<ActionResult<Result>> UpdateDocumentPages(UpdateDocumentPagesDimensionsCommand command)
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
                return Result.Failure($"Update document pages dimensions update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatedocumentpage")]
        public async Task<ActionResult<Result>> ActivateDocumentPage(ActivateDocumentPageCommand command)
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
                return Result.Failure($"Activate document page failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatedocumentpage")]
        public async Task<ActionResult<Result>> DeactivateDocumentPage(DeactivateDocumentPageCommand command)
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
                return Result.Failure($"Deactivate document page failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deletedocumentpage")]
        public async Task<ActionResult<Result>> DeleteDocumentPage(DeleteDocumentPageCommand command)
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
                return Result.Failure($"Delete document page failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        //// [HttpPost("updatedocumentpagestatus")]
        //private async Task<ActionResult<Result>> UpdateDocumentPageStatus(UpdateDocumentPageStatusCommand command)
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
        //        return Result.Failure($"Document page creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
        //    }
        //}

        [HttpGet("getdocumentpagebyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetDocumentPageById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetDocumentPageByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
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

        [HttpGet("getdocumentpagesbydocument/{subscriberid}/{documentid}/{userid}")]
        public async Task<ActionResult<Result>> GetDocumentPagesBySubscription(int subscriberid, int documentid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetDocumentPagesByDocumentQuery
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
                return Result.Failure(new string[] { "Get document pages by document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getdocumentpages/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetDocumentPages(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetDocumentPagesQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all document page was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivedocumentpages/{subscriberid}/{documentId}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveDocumentPages(int subscriberid, int documentId, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveDocumentPagesQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    DocumentId = documentId,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active document page was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getalldocumentpages/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetAllDocumentPages(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetDocumentPagesQuery() { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving document page was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}