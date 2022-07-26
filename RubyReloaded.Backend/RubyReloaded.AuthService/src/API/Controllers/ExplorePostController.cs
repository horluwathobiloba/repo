using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.ExplorePost.Commands.CreateExplorePost;
using RubyReloaded.AuthService.Infrastructure.Utility;
using System.Linq;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.ExplorePost.Commands.UpdateExplorePost;
using RubyReloaded.AuthService.Application.ExplorePost.Queries.GetExplorePost;
using RubyReloaded.AuthService.Application.ExplorePost.Commands.ChangeExplorePostStatus;

namespace API.Controllers
{
    public class ExplorePostController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ExplorePostController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> CreateExplorePost(CreateExplorePostCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                return await Mediator.Send(command);

            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Post creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPut("update/{id}")]
        public async Task<ActionResult<Result>> UpdateExplorePost(int id, [FromBody] UpdateExplorePostCommand command)
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
                return Result.Failure(new string[] { "Post update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetById(int id)
        {
            try
            {
                return await Mediator.Send(new GetExplorePostById { Id = id });
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
        public async Task<ActionResult<Result>> GetAllExplorePost(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetExplorePostsQuery
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

        [HttpPost("changeExplorePosttatus")]
        public async Task<ActionResult<Result>> ChangeExplorePosttatus(ChangeExplorePostStatusCommand command)
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
