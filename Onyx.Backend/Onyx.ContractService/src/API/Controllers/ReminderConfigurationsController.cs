using Onyx.ContractService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ReminderConfigurations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.ContractService.Application.ReminderConfigurations.Queries;
using Microsoft.AspNetCore.Http;
using Onyx.ContractService.Application.ReminderConfiguration.Commands;
using Onyx.ContractService.Domain.Enums;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReminderConfigurationsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ReminderConfigurationsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateReminderConfigurationCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Reminder configuration creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getreminderconfigurationbyid/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetReminderConfigurationById(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetReminderConfigurationsByIdQuery { OrganisationId = organisationid,Id= id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get reminder configuration by Organisation Id failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getreminderconfigurations/{organisationid}")]
        public async Task<ActionResult<Result>> GetReminderConfigurationByOrgId(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetReminderConfigurationsByOrgIdQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get reminder configuration by OrgId failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpGet("getreminderconfigurationsbytype/{organisationid}/{reminderType}")]
        public async Task<ActionResult<Result>> GetReminderConfigurationsByType(int organisationid, ReminderType reminderType)
        {
            try
            {
                return await Mediator.Send(new GetReminderConfigurationsByTypeQuery { OrganisationId = organisationid,ReminderType = reminderType, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get reminder configuration by type failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }



        [HttpDelete("delete/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> Delete(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteReminderConfigurationsCommand { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Reminder configuration failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }
}
