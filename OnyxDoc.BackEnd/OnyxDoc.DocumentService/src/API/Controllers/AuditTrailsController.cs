using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Domain.Enums;
using System.Linq;
using OnyxDoc.DocumentService.Infrastructure.Utility;
using OnyxDoc.DocumentService.Application.AuditTrails.Queries.GetAuditTrail;
using OnyxDoc.DocumentService.Application.Clients.Queries.GetAuditTrail;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AuditTrailsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public AuditTrailsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }



        [HttpGet("getall/{userId}/{subscriberId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetAll(string userId, int subscriberId, int skip, int take)
        {
            try
            {
                return await Mediator.Send(new GetAuditTrailQuery 
                { 
                    UserId = userId,
                    SubscriberId = subscriberId, 
                    Skip = skip, 
                    Take = take,
                    AccessToken = accessToken
                });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Audit Trail was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbycontroller/{userId}/{controllername}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetByController(string userId,string controllername, int skip, int take)
        {
            try
            {
               
                return await Mediator.Send(new GetAuditTrailQueryByController 
                { 
                    UserId = userId,    
                    ControllerName = controllername,
                    Skip = skip, 
                    Take = take,
                    AccessToken = accessToken
                });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Audit Trail by controller was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbyauditaction/{userId}/{subscriberId}/{auditaction}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetByAuditAction(string userId,int subscriberId, AuditAction auditaction, int skip, int take)
        {
            try
            {
                return await Mediator.Send(new GetAuditTrailQueryByAction
                {
                    SubscriberId = subscriberId,
                    UserId = userId,
                    AuditAction = auditaction,
                    Skip = skip,
                    Take = take,
                    AccessToken=accessToken
                });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Audit Trail by audit action was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getbysubscriberid/{userId}/{subscriberId}/{skip}/{take}")]
        public async Task<ActionResult<Result>> GetBySubscriberId(string userId, int subscriberId, int skip, int take)
        {
            try
            {
                return await Mediator.Send(new GetAuditTrailQueryBySubscriberId 
                { 
                    UserId = userId,
                    SubscriberId = subscriberId,
                    Skip = skip,
                    Take = take,
                    AccessToken = accessToken
                });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Audit Trail by controller was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getauditparameters/{userId}/{subscriberId}")]
        public async Task<ActionResult<Result>> GetAuditParameters(string userId, int subscriberId)
        {
            try
            {

                return await Mediator.Send(new GetAuditParametersQuery
                {
                    UserId = userId,
                    SubscriberId = subscriberId,

                });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Audit parameters was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
