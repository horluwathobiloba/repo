using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;  
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Contract.Commands.UploadContract;
using Onyx.ContractService.Application.Contract.Commands.CreateDocumentSigningLink;
using Onyx.ContractService.Application.Contract.Commands.DecodeUrlHash;
using Onyx.ContractService.Application.ContractDocuments.Commands.SendToDocumentSignatories;
using Onyx.ContractService.Application.ContractDocuments.Commands.SaveSignedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Application.Contract.Commands.UploadSupportingDocument;
using Onyx.ContractService.Application.ContractDocuments.Commands.UpdateDimensions;
using Microsoft.AspNetCore.Http;
using System;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractDocumentsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractDocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        /// <summary>
        /// This api allows you to upload contract documents.
        /// </summary>
        /// <param name="command">The UploadContract command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("uploadcontractdocument")]
        public async Task<ActionResult<Result>> UploadContractDocument(UploadContractCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Document Upload was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to upload signing documents.
        /// </summary>
        /// <param name="command">The UploadSupportingDocument command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("uploadsupportingdocument")]
        public async Task<ActionResult<Result>> UploadSupportingDocument(UploadSupportingDocument command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Supporting Document Upload was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
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
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Document Signing Link Creation was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
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
                return await Mediator.Send(new DecodeUrlHashCommand {  DocumentLinkHash = documentHash });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Decode Url Hash was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
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
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Sending Document to signatories was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to save signed document.
        /// </summary>
        /// <param name="command">The SaveSignedDocumentCommand command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("savesigneddocument")]
        public async Task<ActionResult<Result>> SaveSignedDocument(SaveSignedDocumentCommand command)
        {
            try
            { 
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Saving signed document was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        /// <summary>
        /// This api allows you to save update dimensions.
        /// </summary>
        /// <param name="command">The UpdateDimensionsCommand command object</param>
        /// <returns>Returns the Result object either success/failure</returns>
        [HttpPost("updatedimensions")]
        public async Task<ActionResult<Result>> UpdateDimensions(UpdateDimensionsCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"updating dimensions was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

    }
}
