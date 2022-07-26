using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Commands.CreateDocumentSigningLink;
using OnyxDoc.DocumentService.Application.Commands.DecodeUrlHash;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Document.Commands.UpdateDocumentName;
using OnyxDoc.DocumentService.Application.Document.Commands.UploadDocument;
using OnyxDoc.DocumentService.Application.Document.Queries.GetDocument;
using OnyxDoc.DocumentService.Application.Document.Queries.GetDocuments;
using OnyxDoc.DocumentService.Application.Documents.Commands.DeleteDocument;
using OnyxDoc.DocumentService.Application.Documents.Commands.SaveSignedDocument;
using OnyxDoc.DocumentService.Application.Documents.Commands.SendDocumentExpirationNotification;
using OnyxDoc.DocumentService.Application.Documents.Commands.SendToDocumentSignatories;
using OnyxDoc.DocumentService.Application.Documents.Commands.UpdateComponents;
using OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments;

namespace API.Controllers
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


        /// <summary>
        /// This api allows you to upload  documents.
        /// </summary>
        /// <param name="command">The Upload command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("uploaddocument")]
        public async Task<ActionResult<Result>> UploadDocument(UploadDocumentCommand command)
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
                return Result.Failure($"Document Upload was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }



        /// <summary>
        /// This api allows you to create hashed document signing links.
        /// </summary>
        /// <param name="command">The CreateDocumentSigningLink command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("createdocumentsigninglink")]
        public async Task<ActionResult<Result>> Create(CreateDocumentSigningLinkCommand command)
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
                return Result.Failure($"Document Signing Link Creation was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to update document name.
        /// </summary>
        /// <param name="command">The UpdateDocumentName command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("updatedocumentname")]
        public async Task<ActionResult<Result>> UpdateDocumentName(UpdateDocumentNameCommand command)
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
                return Result.Failure($"Updating Document Name was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
        /// <summary>
        /// This api allows you to delete a document.
        /// </summary>
        /// <param name="command">The Delete Document command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("deletedocument")]
        public async Task<ActionResult<Result>> DeleteDocument(DeleteDocumentCommand command)
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
                return Result.Failure($"Deleting Document was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to decode url hash.
        /// </summary>
        /// <param name="command">The DecodeUrlHash command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpGet("decodeurlhash/hash")]
        public async Task<ActionResult<Result>> DecodeUrlHash(string documentHash)
        {
            try
            {
                return await Mediator.Send(new DecodeUrlHashCommand { DocumentLinkHash = documentHash });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Decode Url Hash was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to  send document to signatories with email.
        /// </summary>
        /// <param name="command">The DecodeUrlHash command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("senddocumenttosignatories")]
        public async Task<ActionResult<Result>> SendDocumentToSignatories(SendToDocumentSignatoriesCommand command)
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
                return Result.Failure($"Sending Document to signatories was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to save signed document.
        /// </summary>
        /// <param name="command">The SaveSignedDocumentCommand command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("signandacceptdocument")]
        public async Task<ActionResult<Result>> SaveSignedDocument(SaveSignedDocumentCommand command)
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
                return Result.Failure($"Saving signed document was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to update component.
        /// </summary>
        /// <param name="command">The UpdateComponent command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("updatecomponent")]
        public async Task<ActionResult<Result>> UpdateComponent(UpdateComponentsCommand command)
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
                return Result.Failure($"Updating document was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to update component.
        /// </summary>
        /// <param name="command">The GetBySubscriberId command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpGet("getbysubscriberid/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetBySubscriberId(int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new GetDocumentsBySubscriberIdQuery { SubscriberId = subscriberId, UserId = userId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving documents by subscriber id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getbyid/{Id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int Id, string userId)
        {
            try
            {
                return await Mediator.Send(new GetDocumentByIdQuery { DocumentId = Id, UserId = userId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving document by Id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
        [HttpGet("getdocumentlist/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetDocumentList(int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new GetDocumentsBySubscriberIdQuery { SubscriberId = subscriberId, UserId = userId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Retrieving document list by Subscriber Id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getdocumentsstatusbysubscriberid/{subscriberId}/{userId}/{systemSettingsId}")]

        public async Task<ActionResult<Result>> GetDocumentsStatusBySubscriberId(int subscriberId, string userId, int systemSettingsId)
        {
            try
            {
                return await Mediator.Send(new GetDocumentsStatusBySubscriberIdQuery
                {
                    SubscriberId = subscriberId,
                    UserId = userId,
                    SystemSettingsId = systemSettingsId,
                    AccessToken = accessToken

                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Retrieving documents settings by Id  was not successful. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("senddocumentnotification/{userId}/{subscriberId}/{systemSettingsId}/{documentId}")]
        public async Task<ActionResult<Result>> SendDocumentNotification(string userId, int subscriberId, int systemSettingsId, int documentId)
        {
            try
            {
                return await Mediator.Send(new SendDocumentExpirationNotificationCommand
                {
                    SubscriberId = subscriberId,
                    UserId = userId,
                    SystemSettingId = systemSettingsId,
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
                return Result.Failure($"Sending Document Notification failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }
}
