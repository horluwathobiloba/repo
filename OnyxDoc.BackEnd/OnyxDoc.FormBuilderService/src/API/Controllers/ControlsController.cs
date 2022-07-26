using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.Controls.Commands;
using OnyxDoc.FormBuilderService.Application.Common.Exceptions;
using OnyxDoc.FormBuilderService.Application.Controls.Queries;
using OnyxDoc.FormBuilderService.Domain.Enums;

namespace OnyxDoc.FormBuilderService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ControlsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ControlsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateControlCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Control creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateControlCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Control update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("activatecontrol")]
        public async Task<ActionResult<Result>> ActivateControl(ActivateControlCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Activate control failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("deactivatecontrol")]
        public async Task<ActionResult<Result>> DeactivateControl(DeactivateControlCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Deactivate control failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        //[HttpPost("updateControlstatus")]
        private async Task<ActionResult<Result>> UpdateControlStatus(UpdateControlStatusCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Update control status failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontrolbyid/{subscriberid}/{userid}/{id}")]
        public async Task<ActionResult<Result>> GetControlById(int subscriberid, string userid, int id)
        {
            try
            {
                return await Mediator.Send(new GetControlByIdQuery { SubscriberId = subscriberid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get control failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrolsbytype/{subscriberid}/{userid}/{controltype}")]
        public async Task<ActionResult<Result>> GetControlsByType(int subscriberid, string userid, ControlType controltype)
        {
            try
            {
                return await Mediator.Send(new GetControlsByTypeQuery
                {
                    SubscriberId = subscriberid,
                    ControlType = controltype,
                    UserId = userid,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Get controls by type failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrols/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetControls(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetControlsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all controls was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivecontrols/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveControls(int subscriberid, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActiveControlsQuery { SubscriberId = subscriberid, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active controls was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcontrolbyversionnumber/{subscriberid}/{userid}/{id}/{versionnumber}")]
        public async Task<ActionResult<Result>> GetControlByVersionNumber(int subscriberid, string userid, int id, decimal versionnumber)
        {
            try
            {
                return await Mediator.Send(new GetControlByVersionNumberQuery
                {
                    SubscriberId = subscriberid,
                    UserId = userid,
                    Id = id,
                    VersionNumber = versionnumber,
                    AccessToken = accessToken
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving previous control was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpreviouscontrolversions/{subscriberid}/{userid}/{id}")]
        public async Task<ActionResult<Result>> GetPreviousControlVersions(int subscriberid, string userid, int id)
        {
            try
            {
                return await Mediator.Send(new GetPreviousControlVersionsQuery { SubscriberId = subscriberid, UserId = userid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving previous control was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}