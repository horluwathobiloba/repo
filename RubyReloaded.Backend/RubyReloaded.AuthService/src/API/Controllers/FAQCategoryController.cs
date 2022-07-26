using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.FAQCategory.Commands.CreateFAQCategoryCommand;
using RubyReloaded.AuthService.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.FAQCategory.Commands.ChangeFAQCategoryCommand;
using RubyReloaded.AuthService.Application.FAQCategory.Queries.GetFAQCategories;

namespace API.Controllers
{
    public class FAQCategoryController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public FAQCategoryController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateFAQCategoryCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUser);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Role creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changefaqcategorystatus")]
        public async Task<ActionResult<Result>> ChangeFAQCategoryStatus(ChangeFAQCategoryCommand command)
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

                return Result.Failure(new string[] { "Changing Role status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAll(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetFAQCategoriesQuery { Skip = skip, Take = take });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving  trackers was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetById(int id, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetFAQCategoryById { Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Role by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
