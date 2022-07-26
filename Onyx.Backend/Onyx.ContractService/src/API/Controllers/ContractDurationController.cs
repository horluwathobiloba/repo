using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.ContractDuration.Commands.UpdateContractDuration;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.ContractDuration.Queries.GetContractDurations;
using Onyx.ContractService.Application.ContractDuration.Commands.UpdateContractDurationStatus;
using Onyx.ContractService.Application.ContractDuration.Commands.CreateContractDurations;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    public class ContractDurationController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractDurationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractDurationCommand command)
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
                return Result.Failure($"Vendor creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcontractdurations")]
        public async Task<ActionResult<Result>> CreateContractDurations(CreateContractDurationsCommand command)
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
                return Result.Failure($"Contract Duration creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
        [HttpPost("updatecontractdurationstatus")]
        public async Task<ActionResult<Result>> UpdateContractDurationStatus(UpdateContractDurationStatusCommand command)
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
                return Result.Failure($"Contract Duration creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateContractDurationCommand command)
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

                return Result.Failure($"Contract Duration update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        //[HttpPost("updateVendorstatus")]
        //public async Task<ActionResult<Result>> UpdateVeStatus(U command)
        //{
        //    try
        //    {
        //        return await Mediator.Send(command);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure($"Changing vendor status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
        //    }
        //}

        [HttpGet("getcontractdurations/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractDuration(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractDurationsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract duration retrieval failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractdurationbyid/{id}/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractDurationById(int id, int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractDurationById { Id = id, OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get contract duration by failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
    }
}
