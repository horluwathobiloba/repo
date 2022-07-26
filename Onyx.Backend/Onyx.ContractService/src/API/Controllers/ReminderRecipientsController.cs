using Onyx.ContractService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ReminderRecipients.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.ContractService.Application.ReminderRecipients.Queries;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReminderRecipientsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ReminderRecipientsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateReminderRecipientsCommand command)
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
                return Result.Failure($"Reminder recipient creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getreminderrecipientbyid/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetReminderRecipientById(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetReminderRecipientsByIdQuery { OrganisationId = organisationid,Id= id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get reminder recipient by OrgId & Contract Id failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getreminderrecipients/{organisationid}")]
        public async Task<ActionResult<Result>> GetReminderRecipientByOrgId(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetReminderRecipientsByOrgIdQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get reminder recipient by OrgId failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getreminderrecipientsbycontractid/{organisationid}/{contractid}")]
        public async Task<ActionResult<Result>> GetReminderRecipientsByContractId(int organisationid, int contractid)
        {
            try
            {
                return await Mediator.Send(new GetReminderRecipientsByContractIdQuery { OrganisationId = organisationid, ContractId = contractid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get reminder recipient by ContractId failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getallreminderrecipient/{organisationid}")]
        public async Task<ActionResult<Result>> GetAllReminderRecipient(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetAllReminderRecipientsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get all reminder recipient failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpDelete("delete/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> Delete(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteReminderRecipientsCommand { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Reminder recipient failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }
}
