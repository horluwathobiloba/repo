using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.CreateAddressBook;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBook;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.DeleteAddressBook;
using OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBookStatus;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Application.Dashboard.CreateDashboardDetails;
using Microsoft.AspNetCore.Http;
using OnyxDoc.DocumentService.Application.Dashboard.Queries.GetDocument;
using OnyxDoc.DocumentService.Domain.Enums;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateDashboardCommand command)
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
                return Result.Failure($"Dashboard creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getrecentdocuments/{subscriberId}/{userid}")]
        public async Task<ActionResult<Result>> GetRecentDocuments(int subscriberId, string userid)
        {
            try
            {
                return await Mediator.Send(new GetRecentDocQuery { SubscriberId = subscriberId, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get recent document failed. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }

        [HttpGet("getactivities/{subscriberId}/{selecttype}/{userid}")]
        public async Task<ActionResult<Result>> GetActivities(int subscriberId, DropDownType selecttype, string userid)
        {
            try
            {
                return await Mediator.Send(new GetActivitiesQuery { SubscriberId = subscriberId, DropDownType = selecttype, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get activities failed. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }

        [HttpGet("getdocumentlinechart/{subscriberId}/{year}/{userid}")]
        public async Task<ActionResult<Result>> GetDocumentLineChart(int subscriberId, int year, string userid)
        {
            try
            {
                return await Mediator.Send(new DocumentLineChartQuery { SubscriberId = subscriberId, Year = year, UserId = userid,  AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get document line chart failed. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }

        [HttpGet("getdocumentdonutchart/{subscriberId}/{dropdowntype}/{userid}")]
        public async Task<ActionResult<Result>> GetDocumentDonutChart(int subscriberId, DropDownType dropdowntype, string userid)
        {
            try
            {
                return await Mediator.Send(new DocumentDonutChartQuery { SubscriberId = subscriberId, DropDownType = dropdowntype, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get document donut chart failed. Eror: {ex?.Message + " " + ex?.InnerException.Message}");
            }
        }
    }

}
