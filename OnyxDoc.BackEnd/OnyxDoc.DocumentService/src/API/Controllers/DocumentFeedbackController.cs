using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.DocumentFeedback.Commands.CreateDocumentFeedback;
using OnyxDoc.DocumentService.Application.DocumentFeedback.Queries.GetDocumentFeedback;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DocumentFeedbackController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public DocumentFeedbackController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("createfeedback")]
        public async Task<ActionResult<Result>> CreateFeedback(CreateDocumentFeedbackCommand command)
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
                return Result.Failure($"Creation of Document Feedback failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getall")]
        public async Task<ActionResult<Result>> GetAll()
        {
            try
            {
                return await Mediator.Send(new GetAllDocumentFeedbackQuery { AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Error getting all documents feedback. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }

        [HttpGet("getdocumentfeedbackbyid/{subscriberId}/{userId}/{id}")]
        public async Task<ActionResult<Result>> GetDocumentFeedbackById(int subscriberId, int id, string userId)
        {
            try
            {
                return await Mediator.Send(new GetDocumentFeedbackByIdQuery { SubscriberId = subscriberId, UserId = userId, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Getting document feedback by Id failed. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }

        [HttpGet("getdocumentfeedbackbyuserid/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetDocumentFeedbackByUserId(int subscriberId, string userId)
        {
            try
            {
                return await Mediator.Send(new GetDocumentFeedbackByUserIdQuery { SubscriberId = subscriberId, UserId = userId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get  document feedback by User Id failed. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }
}
