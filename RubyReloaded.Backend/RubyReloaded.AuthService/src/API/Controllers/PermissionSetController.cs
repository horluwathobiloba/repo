using RubyReloaded.AuthService.Application.PermissionSets.Commands.CreatePermissionSet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using RubyReloaded.AuthService.Infrastructure.Utility;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.PermissionSets.Queries.GetPermissionSets;
using RubyReloaded.AuthService.Application.PermissionSets.Commands.ChangePermissionSet;
using RubyReloaded.AuthService.Application.PermissionSets.Commands.UpdatePermissionSet;
using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Domain.Enums;
namespace RubyReloaded.AuthService.API.Controllers
{
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   
    public class PermissionSetsController : ApiController       
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PermissionSetsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreatePermissionSetCommand command)
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
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "PermissionSet creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

   
        [HttpPost("update/{featureId}")]
        public async Task<ActionResult<Result>> Update(int featureId, UpdatePermissionSetCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                if (featureId != command.PermissionSetId || (featureId == 0 || command.PermissionSetId == 0))
                {
                    return BadRequest("Invalid PermissionSet Id");
                }
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "PermissionSet update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }

    

        [HttpPost("changefeaturestatus")]
        public async Task<ActionResult<Result>> ChangePermissionSetStatus(ChangePermissionSetStatusCommand command)
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

                return Result.Failure(new string[] { "Changing PermissionSet status was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getall/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(string userId,int skip, int take)
        {
            try
            {
               
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetPermissionSetsQuery { Skip=skip, Take=take});
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving  PermissionSets was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyid/{id}/{userId}")]
        public async Task<ActionResult<Result>> GetById(int id,string userId)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Please input valid feature Id");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetPermissionSetByIdQuery { Id = id });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving PermissionSet by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


   
        [HttpGet("getbyname/{name}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetByName(string name,string userId, int skip, int take)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Please input valid feature name");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetPermissionSetByNameQuery { Name = name, Skip=skip, Take=take });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving PermissionSet by name was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }



        [HttpGet("getbyaccesslevel/{accessLevel}/{userId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetPermissionSetPermissions(AccessLevel accessLevel, string userId, int skip, int take)
        {
            try
            {
                if (accessLevel <= 0)
                {
                    return BadRequest("Please input valid access level");
                }
                accessToken.ValidateToken(userId);
                return await Mediator.Send(new GetPermissionSetsByAccessLevelQuery { AccessLevel = accessLevel, Skip=skip, Take=take });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving all PermissionSets by access level was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
