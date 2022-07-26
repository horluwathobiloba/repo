using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.Documents.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Documents.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public DocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateDocumentCommand command)
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
                return Result.Failure($"Document creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateDocumentCommand command)
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
                return Result.Failure($"Document update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatedocument")]
        public async Task<ActionResult<Result>> ActivateDocument(ActivateDocumentCommand command)
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
                return Result.Failure($"Activate document failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatedocument")]
        public async Task<ActionResult<Result>> DeactivateDocument(DeactivateDocumentCommand command)
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
                return Result.Failure($"Deactivate document failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        //[HttpPost("updatedocumentstatus")]
        private async Task<ActionResult<Result>> UpdateDocumentStatus(UpdateDocumentStatusCommand command)
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
                return Result.Failure($"Update document status failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getdocumentbyid/{subscriberid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetDocumentById(int subscriberid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetDocumentByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get document failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getdocumentsbytype/{subscriberid}/{userid}/{documenttype}")]
        public async Task<ActionResult<Result>> GetDocumentsByTypeQuery(int subscriberid, string userid, DocumentType documenttype)
        {
            try
            {
                return await Mediator.Send(new GetDocumentsByTypeQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    DocumentType = documenttype,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get documents by type failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getdocuments/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetDocuments(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetDocumentsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all documents was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivedocuments/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveDocuments(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveDocumentsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active documents was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getdocumentbyversionnumber/{subscriberid}/{userid}/{id}/{versionnumber}")]
        public async Task<ActionResult<Result>> GetDocumentByVersionNumber(int subscriberid, string userid, int id, decimal versionnumber)
        {
            try
            {
                return await Mediator.Send(new GetDocumentByVersionNumberQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    Id = id,
                    VersionNumber = versionnumber,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving previous document was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpreviousdocumentversions/{subscriberid}/{userid}/{id}")]
        public async Task<ActionResult<Result>> GetPreviousDocumentVersions(int subscriberid, string userid, int id)
        {
            try
            {
                return await Mediator.Send(new GetPreviousDocumentVersionsQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    Id = id,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving previous document was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}