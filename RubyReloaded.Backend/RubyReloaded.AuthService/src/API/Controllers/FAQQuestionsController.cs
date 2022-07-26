using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.FAQQuestion.Commands.CreateFAQQuestions;
using RubyReloaded.AuthService.Infrastructure.Utility;
using System;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.FAQQuestion.Commands.UpdateFAQQuestion;
using RubyReloaded.AuthService.Application.FAQQuestion.Queries.GetFAQQuestionsQuery;
using RubyReloaded.AuthService.Application.FAQQuestion.Commands.ChangeFAQQuestionStatus;

namespace API.Controllers
{
    public class FAQQuestionsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public FAQQuestionsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateFAQQuestion(CreateFAQQuestionsCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);

            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "FAQ Question creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPut("update/{id}")]
        public async Task<ActionResult<Result>> UpdateFAQQuestion(int id, [FromBody] UpdateFAQQuestionCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "FAQ Question update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {
                return await Mediator.Send(new GetFAQQuestionByIdQuery { Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving FAQ Question by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAllFAQQuestions(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAllFAQQuestionsQuery
                {
                    Skip = skip,
                    Take = take
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all FAQ Questions was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getfaqquestionsbycategoryid/{categoryid}/{userid}")]
        public async Task<ActionResult<Result>> GetFAQQuestionsByCategoryId(int categoryid, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetFAQByCategoryIdQuery
                {
                    Id=categoryid
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all FAQ Questions was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changefaqquestionstatus")]
        public async Task<ActionResult<Result>> ChangeFAQQuestionStatus(ChangeFAQQuestionStatusCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Changing FAQ Question status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
