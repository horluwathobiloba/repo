using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.RequestToJoinTrackers.Commands;
using System;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.RequestToJoinTrackers.Queries.GetRequestToJoinTracker;
using RubyReloaded.AuthService.Application.RequestToJoinTrackers.Commands.ChangeRequestToJoinStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestToJoinTrackerController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public RequestToJoinTrackerController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(AddRequestToJoinTrackerCommand command)
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
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Role creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
       
        [HttpPost("changerequesttojoinstatus")]
        public async Task<ActionResult<Result>> ChangeRoleStatus(ChangeRequestToJoinStatusCommand command)
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
                return await Mediator.Send(new GetAllRequestToJoinTracker { Skip=skip,Take=take});
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
        public async Task<ActionResult<Result>> GetById(int id,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetRequestToJoinById { Id = id });
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
        [HttpGet("getbycooperativeid/{cooperativeid}/{userid}")]
        public async Task<ActionResult<Result>> GetByCooperativeId(int cooperativeid,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                if (cooperativeid <= 0)
                {
                    return BadRequest("Please input valid cooperative Id");
                }
                return await Mediator.Send(new GetRequestTrackersByCooperativeId { CooperativeId = cooperativeid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Role by organization Id  was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        

    }
}
