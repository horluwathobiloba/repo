using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Cooperatives.Commands.CreateCooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Cooperatives.Commands.UpdateCooperative;
using RubyReloaded.AuthService.Application.Cooperatives.Commands.ChangeCooperativeStatus;
using RubyReloaded.AuthService.Application.Cooperatives.Queries.GetCooperatives;
using RubyReloaded.AuthService.Application.Cooperatives.Commands.CooperativeSignUp;
using RubyReloaded.AuthService.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CooperativeController:ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CooperativeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateCooperativeCommand command)
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

                return Result.Failure(new string[] { "Cooperative creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("signup")]
        public async Task<ActionResult<Result>> SignUp(CooperativeSignUpCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Cooperative signUp was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("update/{cooperativeid}")]
        public async Task<ActionResult<Result>> Update(string cooperativeId, UpdateCooperativeCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                if (cooperativeId==null)
                {
                    return BadRequest("Invalid CooperativeId Id");
                }

                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Cooperative update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changecooperativestatus")]
        public async Task<ActionResult<Result>> ChangeCooperativeStatus(ChangeCooperativeStatusCommand command)
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
                return Result.Failure(new string[] { "Changing Cooperative Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAll(int skip, int take,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetCooperativesQuery { 
                Skip=skip,
                Take=take
                });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Cooperative retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcooperativebytype/type")]
        public async Task<ActionResult<Result>> GetCooperativeByCooperatibeType(CooperativeType type)
        {
            try
            {
                
                return await Mediator.Send(new GetCooperativesByCooperativeType { CooperativeType=type});
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Cooperative retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbycooperativeid/{cooperativeid}/{userid}")]
        public async Task<ActionResult<Result>> GetByCooperativeId(int cooperativeid,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetCooperativeByCooperativeIdQuery { CooperativeId = cooperativeid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Cooperative retrieval by organization id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
