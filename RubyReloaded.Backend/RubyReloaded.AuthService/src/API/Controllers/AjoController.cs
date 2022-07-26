using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Ajos.Command;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Ajos.Command.AjoSignUp;
using RubyReloaded.AuthService.Application.Ajos.Command.UpdateAjo;
using RubyReloaded.AuthService.Application.Ajos.Command.ChangeAjoStatus;
using RubyReloaded.AuthService.Application.Ajos.Queries;
using RubyReloaded.AuthService.Application.Ajos.Queries.GetAjos;
using RubyReloaded.AuthService.Application.AjoMembers.Command.CreateAjoMember;
using RubyReloaded.AuthService.Application.AjoMembers.Command.ChangeAjoMemberStatus;
using RubyReloaded.AuthService.Application.Ajos.Command.SendAjoInvitationCode;
using RubyReloaded.AuthService.Application.Ajos.Command.VerifyAjoInvitationCode;
using RubyReloaded.AuthService.Application.Ajos.Command.CreateAjoInviteLink;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.AuthService.Infrastructure.Utility;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AjoController: ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public AjoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateAjoCommand command)
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

                return Result.Failure(new string[] { "Ajo creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("createajomember")]
        public async Task<ActionResult<Result>> CreateAjoMember(CreateAjoMemberCommand command)
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

                return Result.Failure(new string[] { "Ajo creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("createajoinvitationlink")]
        public async Task<ActionResult<Result>> CreateAjoInvitationLink(CreateAjoInviteLinkCommand command)
        {
            try
            {
                //accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Ajo Invite link creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("createajouser")]
        public async Task<ActionResult<Result>> CreateAjoUser(CreateAjoUserCommand command)
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

                return Result.Failure(new string[] { "Ajo Invite link creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("changeajomemberstatus")]
        public async Task<ActionResult<Result>> ChangeAjoMemberStatus(ChangeAjoMemberStatusCommand command)
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

                return Result.Failure(new string[] { "Change status operation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("sendajoinvitationcode")]
        public async Task<ActionResult<Result>> SendAjoInvitationCode(SendAjoInvitationCodeCommand command)
        {
            try
            {
               // accessToken.ValidateToken(command.LoggedInUserId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Ajo code creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpPost("verifyajoinvitationcode")]
        public async Task<ActionResult<Result>> VerifyAjoInvitationCode(VerifyAjoInvitationCodeCommand command)
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

                return Result.Failure(new string[] { "Ajo code creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<Result>> SignUp(AjoSignUpCommand command)
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

                return Result.Failure(new string[] { "Ajo signUp was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("update/{ajoid}")]
        public async Task<ActionResult<Result>> Update(string ajoId,UpdateAjoCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggedInUserId);
                if (ajoId == null)
                {
                    return BadRequest("Invalid ajo Id");
                }

                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Ajo update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changeajostatus")]
        public async Task<ActionResult<Result>> ChangeAjoStatus(ChangeAjoStatusCommand command)
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
                return Result.Failure(new string[] { "Changing Ajo Status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAll(int skip, int take,string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAllAjos { 
                Skip=skip,
                Take=take
                });
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Ajo retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyAjoid/{ajoid}")]
        public async Task<ActionResult<Result>> GetByAjoId(int ajoid)
        {
            try
            {
                //accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAjoById { Id = ajoid });
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Ajo retrieval by organization id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
