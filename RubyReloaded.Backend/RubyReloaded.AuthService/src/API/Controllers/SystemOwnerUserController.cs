using Microsoft.AspNetCore.Mvc;
using RubyReloaded.AuthService.API.Controllers;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.SuperAdmin.Command.CreateSuperAdmin;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RubyReloaded.AuthService.Application.SuperAdmin.Command.UpdateSuperAdmin;
using RubyReloaded.AuthService.Application.SuperAdmin.Command.ChangeSuperAdmiUserStatus;
using Microsoft.AspNetCore.Http;
using RubyReloaded.AuthService.Infrastructure.Utility;
using RubyReloaded.AuthService.Application.SuperAdmin.Queries.GetSuperAdminUsers;
using RubyReloaded.AuthService.Application.SystemOwnerUser;

namespace API.Controllers
{
  // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SystemOwnerUserController:ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public SystemOwnerUserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateSystemOwnerUserCommand command)
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
                return Result.Failure(new string[] { "User creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpPost("createsystemusers")]
        public async Task<ActionResult<Result>> Create(CreateSystemOwnerUsersCommand command)
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
                return Result.Failure(new string[] { "User creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateSystemOwnerUserCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.LoggeInUser);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {

                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "User update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }  
        [HttpPost("changesuperadminstatus")]
        public async Task<ActionResult<Result>> ChangeStatus(ChangeSystemOwnerUserStatusCommand command)
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

                return Result.Failure(new string[] { "User update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("getall/{skip}/{take}/{userid}")]
        public async Task<ActionResult<Result>> GetAll(int skip, int take, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetAllSystemOwnerUsers
                {
                    Skip = skip,
                    Take = take
                });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyid/{email}/{useraccesslevel}/{userid}")]
        public async Task<ActionResult<Result>> GetById(string email,int useraccesslevel, string userid)
        {
            try
            {
                accessToken.ValidateToken(userid);
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Please input valid user Id");
                }
               
                return await Mediator.Send(new GetSystemOwnerUserByEmail { Email = email,UserAccessLevel=useraccesslevel });
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "User retrieval was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
