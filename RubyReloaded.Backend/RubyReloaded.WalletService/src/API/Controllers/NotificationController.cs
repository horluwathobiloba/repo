using Microsoft.AspNetCore.Mvc;
using RubyReloaded.WalletService.API.Controllers;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Notifications.Commands.CreateNotification;
using System;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubyReloaded.WalletService.Application.Notifications.Commands.UpdateNotification;
using RubyReloaded.WalletService.Application.Notifications.Commands.ChangeNotoficationStatus;
using RubyReloaded.WalletService.Application.Notifications.Queries.GetNotification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateNotificationCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Notification creation was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Result>> Update(string id, [FromBody] UpdateNotificationCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Notification update was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


        [HttpPost("changenotificationstatus")]
        public async Task<ActionResult<Result>> ChangeNotificationStatus(ChangeNotificationStatusCommand command)
        {
            try
            {
              

                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Changing Notification was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        [HttpGet("readnotifications/{customerId}/{deviceNotificationId}")]
        public async Task<ActionResult<Result>> ReadNotificationsByCustomerId(string customerId, string deviceNotificationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return BadRequest("Please input valid customer Id");
                }

                return await Mediator.Send(new GetReadNotificationsByUserIdQuery { UserId = customerId, DeviceNotificationId = deviceNotificationId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Getting read notification count by customer id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("unreadnotifications/{customerId}/{deviceNotificationId}")]
        public async Task<ActionResult<Result>> UnReadNotificationsByCustomerId(string customerId, string deviceNotificationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return BadRequest("Please input valid customer Id");
                }

                return await Mediator.Send(new GetUnReadNotificationsByUserIdQuery { UserId = customerId, DeviceNotificationId = deviceNotificationId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Getting unread notification count by customer id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
        //[HttpGet]
        //public async Task<ActionResult<Result>> PushNotification()
        //{
        //    try
        //    {
        //       return await Mediator.Send(new PushNotificationCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result.Failure( "Push Notifications not successful" + ex?.Message ?? ex?.InnerException?.Message );
        //    }
        //  }
    }
}
