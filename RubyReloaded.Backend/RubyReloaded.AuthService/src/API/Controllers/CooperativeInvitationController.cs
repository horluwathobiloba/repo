using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Infrastructure.Utility;
using System;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.CooperativeInvitationTrackers.Command.AddCooperativeInvitationTracker;
using RubyReloaded.AuthService.Application.CooperativeInvitationTrackers.Queries.GetCooperativeInvitationTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CooperativeInvitationController:ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CooperativeInvitationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(AddCooperativeInvitationTrackerCommand command)
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
        public async Task<ActionResult<Result>> GetAll(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAllCooperativeInvitationTrackers
                {
                    Skip = skip,
                    Take = take
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
        [HttpGet("getbyajoid/{cooperativeid}/{userid}")]
        public async Task<ActionResult<Result>> GetByAjoId(int cooperativeid, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                if (cooperativeid <= 0)
                {
                    return BadRequest("Please input valid cooprative Id");
                }
                return await Mediator.Send(new GetAllCooperativeInvitatonTrackerByCooperativeId { CooperativeId = cooperativeid });
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
