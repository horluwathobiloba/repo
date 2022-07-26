using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnyxDoc.AuthService.Application.Branding.Commands.CreateBranding;
using OnyxDoc.AuthService.Application.Branding.Commands.UpdateBranding;
using OnyxDoc.AuthService.Application.Branding.Queries.GetBranding;
using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Customizations.Commands.ExpirationReminder.DeleteExpirationReminder;
using OnyxDoc.AuthService.Application.SystemSetting.Commands.CreateSystemSetting;
using OnyxDoc.AuthService.Application.SystemSetting.Commands.UpdateSystemSetting;
using OnyxDoc.AuthService.Application.SystemSetting.Queries.GetSystemSetting;
using OnyxDoc.AuthService.Infrastructure.Utility;
using System;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class BrandingsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BrandingsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("createbranding")]
        public async Task<ActionResult<Result>> CreateBranding(CreateBrandingCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId, command.SubscriberId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Branding creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("updatebranding/{brandingid}")]
        public async Task<ActionResult<Result>> UpdateBranding(int brandingid, UpdateBrandingCommand command)
        {
            try
            {
                
                accessToken.ValidateToken(command.UserId);
                if (brandingid != command.Id || (brandingid == 0 || command.Id == 0))
                {
                    return BadRequest("Invalid Branding Id");
                }
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Brand update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }


        [HttpGet("getbrandingbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetBrandingById(int id, string userid)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Please input valid brand Id");
                }
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetBrandingByIdQuery { Id = id, UserId = userid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Branding by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getbrandingbysubscriberid/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetBrandingBySubscriberId(int subscriberId, string userid)
        {
            try
            {
                if (subscriberId <= 0)
                {
                    return BadRequest("Please input valid brand Id");
                }
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetBrandingBySubscriberIdQuery { SubscriberId = subscriberId, UserId = userid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Branding by Subscriber Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getactivebrandingbysubscriberid/{subscriberId}/{userId}")]
        public async Task<ActionResult<Result>> GetActiveBrandingBySubscriberId(int subscriberId, string userid)
        {
            try
            {
                if (subscriberId <= 0)
                {
                    return BadRequest("Please input valid brand Id");
                }
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetActiveBrandingBySubscriberIdQuery { SubscriberId = subscriberId, UserId = userid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Active Branding by Subscriber Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPost("createsystemsetting")]
        public async Task<ActionResult<Result>> CreateSystemSetting(CreateSystemSettingCommand command)
        {
            try
            {
                if (command == null)
                {
                    return BadRequest("Please fill in required field");
                }
                accessToken.ValidateToken(command.UserId, command.SubscriberId);
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "System setting creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("updatesystemsetting/{systemsettingid}")]
        public async Task<ActionResult<Result>> Update(int systemsettingid, UpdateSystemSettingCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                if (systemsettingid != command.Id || (systemsettingid == 0 || command.Id == 0))
                {
                    return BadRequest("Invalid System Setting Id");
                }
                return await Mediator.Send(command);
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { " System Setting update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }

        }


        [HttpGet("getbysystemsettingbyid/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetSystemSettingById(int id, string userid)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Please input valid system setting Id");
                }
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetSystemSettingByIdQuery { Id = id, UserId = userid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving System Setting by Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsystemsettingbysubscriberid/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetSystemSettingBySubscriberId(int subscriberid, string userid)
        {
            try
            {
                if (subscriberid <= 0)
                {
                    return BadRequest("Please input valid subscriber Id");
                }
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetSystemSettingBySubscriberIdQuery { SubscriberId = subscriberid, UserId = userid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving System Setting by Subscriber Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpGet("getactivesystemsettingbysubscriberid/{subscriberid}/{userid}")]
        public async Task<ActionResult<Result>> GetActiveSystemSettingBySubscriberId(int subscriberid, string userid)
        {
            try
            {
                if (subscriberid <= 0)
                {
                    return BadRequest("Please input valid subscriber Id");
                }
                accessToken.ValidateToken(userid);
                return await Mediator.Send(new GetActiveSystemSettingBySubscriberIdQuery { SubscriberId = subscriberid, UserId = userid });
            }

            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure(new string[] { "Retrieving Active System Setting by Subscriber Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpDelete("deleteExpiryReminder/{id}/{subscriberId}/{systemSettingId}/{userId}")]
        public async Task<ActionResult<Result>> DeleteExpiryReminder(int id, int subscriberId, int systemSettingId, string userId)
        {
            try
            {
                if (subscriberId <= 0)
                {
                    return BadRequest("Invalid Subscriber");
                }

                accessToken.ValidateToken(userId);
                return await Mediator.Send(new DeleteExpirationReminderCommand
                {
                    Id = id,
                    SubscriberId = subscriberId,
                    SystemSettingsId = systemSettingId,
                    UserId = userId
                });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] {"Deleting Expiry Reminder not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

    }
}
