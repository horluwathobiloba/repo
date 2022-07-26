using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Domain.Enums;
using OnyxDoc.DocumentService.Application.Comments.Queries;
using OnyxDoc.DocumentService.Application.Comments.Commands.CreateComment;
using OnyxDoc.DocumentService.Application.Comments.Queries.GetComments;
using OnyxDoc.DocumentService.Application.Comments.Commands.DeleteComment;
using OnyxDoc.DocumentService.Application.Comments.Commands.UpdateComment;
using OnyxDoc.DocumentService.Application.Comments.Commands.UpdateCommentStatus;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CommentsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateCommentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Comment creation was not successful. Error: {ex?.Message +" "+ ex?.InnerException.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateCommentCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
               return Result.Failure($"Comment update failed. Error: {ex?.Message +" "+ ex?.InnerException.Message }");
            }
        }

       
        [HttpPost("updatecommentstatus")]
        public async Task<ActionResult<Result>> UpdateCommentStatus(UpdateCommentStatusCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Changing comment status was not successful. Error: { ex?.Message +" "+ ex?.InnerException.Message }");
            }
        }

        [HttpGet("getcommentsbysubscriberid/{subscriberId}")]
        public async Task<ActionResult<Result>> GetCommentsBySubscriberId(int subscriberId)
        {
            try
            {
                return await Mediator.Send(new GetCommentsBySubscriberIdQuery { SubscriberId = subscriberId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"comments retrieval by subscriber id was not successful. Eror: {ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
        [HttpGet("getcommentsbydocumentid/{subscriberId}/{documentId}")]
        public async Task<ActionResult<Result>> GetCommentsByDocumentId(int subscriberId, int documentId)
        {
            try
            {
                return await Mediator.Send(new GetCommentsByDocumentIdQuery { SubscriberId = subscriberId, DocumentId = documentId, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"comments retrieval by document id was not successful. Eror: {ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
        [HttpGet("getcommentbyid/{subscriberId}/{id}")]
        public async Task<ActionResult<Result>> GetCommentById(int subscriberId, int id)
        {
            try
            {
                return await Mediator.Send(new GetCommentByIdQuery { SubscriberId = subscriberId, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get  comment by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }

        [HttpGet("dynamicsearch/{subscriberId}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int subscriberId, string searchtext)
        {
            try

            {
                return await Mediator.Send(new GetCommentsDynamicQuery { SubscriberId = subscriberId, SearchText = searchtext });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get  comments failed. Eror: {ex?.Message +" "+ ex?.InnerException.Message}");
            }
        }
        [HttpDelete("deletecomment/{subscriberId}/{id}")]
        public async Task<ActionResult<Result>> Delete(int subscriberId, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteCommentCommand { SubscriberId = subscriberId, Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Comment Delete failed. Error: {ex?.Message +" "+ ex?.InnerException.Message }");
            }
        }

        [HttpGet("getcommentsbycommenttype/{subscriberId}/{documentId}/{commentType}")]
        public async Task<ActionResult<Result>> GetCommentsByTypeQuery(int subscriberId, int documentId, CommentType commentType)
        {
            try
            {
                return await Mediator.Send(new GetCommentsByTypeQuery {  SubscriberId = subscriberId,  DocumentId = documentId, CommentType = commentType, AccessToken= accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Getting  Comment by comment type failed. Error: {ex?.Message +" "+ ex?.InnerException.Message }");
            }
        }

       
    }
}
