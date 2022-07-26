using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.AjoInvitationTrackers.Commands.AddAjoInvitationTracker;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.AjoInvitationTrackers.Queries.GetAjoInvitationTracker;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AjoInvitationTrackerController: ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public AjoInvitationTrackerController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(AddAjoInvitationTrackerCommand command)
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

        
        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAll(int skip,int take,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAllAjoInvitationTrackers { 
                Skip=skip,
                Take=take
                });
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

        //[HttpGet("getbyid/{id}")]
        //public async Task<ActionResult<Result>> GetById(int id)
        //{
        //    try
        //    {

        //        return await Mediator.Send(new GetRequestToJoinById { Id = id });
        //    }
        //    catch (ValidationException ex)
        //    {

        //        return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure(new string[] { "Retrieving Role by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
        //    }
        //}
        [HttpGet("getbyajoid/{ajoid}/{userid}")]
        public async Task<ActionResult<Result>> GetByAjoId(int ajoid,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                if (ajoid <= 0)
                {
                    return BadRequest("Please input valid organization Id");
                }
                return await Mediator.Send(new GetAjoInvitationTrackerByAjoId { AjoId = ajoid });
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
