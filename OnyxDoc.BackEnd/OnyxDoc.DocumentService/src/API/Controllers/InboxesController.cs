using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Inboxes.Commands.CreateInboxes;
using System;
using System.Collections.Generic;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using OnyxDoc.DocumentService.Application.Inboxes.Commands.UpdateInboxStatus;
using OnyxDoc.DocumentService.Application.Inboxes.Queries.GetInboxes;
using OnyxDoc.DocumentService.Application.Inboxes.Queries.GetInboxesByEmail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InboxesController : ApiController
    {

        protected readonly IHttpContextAccessor _httpContextAccessor;
        public InboxesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateInboxes(CreateInboxesCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Inboxes creation failed. Error: {ex?.Message + " " + ex?.InnerException.Message }");
            }
        }
        [HttpPost("updateinboxtoreadstatus")]
        public async Task<ActionResult<Result>> UpdateInboxToReadStatus(UpdateInBoxStatusCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Changing Inbox status failed. Error: { ex?.Message + " " + ex?.InnerException.Message }");
            }
        }

        [HttpGet("getinboxes/{subscriberId}/{userId}/{documentId}")]
        public async Task<ActionResult<Result>> GetInboxes(int subscriberId, string userId, int documentId)
        {
            try
            {
                return await Mediator.Send(new GetAllInboxes { SubscriberId = subscriberId, UserId = userId, DocumentId = documentId});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Inbox retrieval failed. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
        [HttpGet("getinboxesbyemail/{subscriberId}/{email}")]
        public async Task<ActionResult<Result>> GetInboxesByEmail(string email)
        {
            try
            {
                return await Mediator.Send(new GetInboxesByEmailQuery {Email = email });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Inboxes by email failed. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }

        [HttpGet("getinboxid/{id}/{documentId}")]
        public async Task<ActionResult<Result>> GetInboxById(  int id, int documentId)
        {
            try
            {
                return await Mediator.Send(new GetInboxByIdQuery {  Id = id, DocumentId = documentId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Inbox by id failed. Error: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
