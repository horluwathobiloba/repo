using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.Inboxes.Queries.GetInboxes;
using Onyx.ContractService.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox;
using Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes;
using Onyx.ContractService.Application.Inboxes.Commands.UpdateInbox;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Onyx.ContractService.Application.Inboxes.Commands.UpdateInboxStatus;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InboxesController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public InboxesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateInboxCommand command)
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
                return Result.Failure($"Inbox creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createinboxes")]
        public async Task<ActionResult<Result>> CreateInboxes(CreateInboxesCommand command)
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
                return Result.Failure($"Inboxes creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateInboxCommand command)
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

                return Result.Failure($"Inbox creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


        [HttpGet("getinboxes/{organisationid}")]
        public async Task<ActionResult<Result>> GetInboxes(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetInboxesQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Inbox retrieval failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getinboxid/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetInboxById(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetInboxByIdQuery { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Inbox by failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getinboxesbyemail/{organisationid}/{email}")]
        public async Task<ActionResult<Result>> GetInboxesByEmail(int organisationid, string email)
        {
            try
            {
                return await Mediator.Send(new GetInboxesByNameQuery { OrganisationId = organisationid, Email = email, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Inboxes by email failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpGet("getinboxesbyreadstatus/{organisationid}/{email}")]
        public async Task<ActionResult<Result>> GetInboxesByReadStatus(int organisationid, string email)
        {
            try
            {
                return await Mediator.Send(new GetInboxesByReadStatus { OrganizationId = organisationid, Email = email, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Inboxes by read failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpPost("updateinboxtoreadstatus")]
        public async Task<ActionResult<Result>> UpdateInboxToReadStatus(UpdateInboxStatusCommand command)
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
                return Result.Failure($"Changing Inbox status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

    }
}
