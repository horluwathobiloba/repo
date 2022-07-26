
using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.Contracts.Commands.CreateContract;
using Onyx.ContractService.Domain.Enums;
using Onyx.ContractService.Application.Contracts.Queries.GetContracts;
using Onyx.ContractService.Application.Contracts.Commands.UpdateStatus;
using Onyx.ContractService.Application.Contracts.Commands.UpdateContractStatus;
using Onyx.ContractService.Application.Contracts.Commands.UpdateExecutedContract;
using Onyx.ContractService.Application.Contracts.Queries.GetExecutedContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Application.ContractRecipientActions.Commands.ApproveAction;
using Onyx.ContractService.Application.Contracts.Commands.UpdateContract;
using Onyx.ContractService.Application.Contracts.Commands.ContractEmailNotification;
using Onyx.ContractService.Application.Contracts.Queries;
using Onyx.ContractService.Application.DashBoards;
using Microsoft.AspNetCore.Http;
using System;
using Onyx.ContractService.Application.Reports.Queries.GetReports;
using Onyx.ContractService.Application.Reports.Queries;
using Onyx.ContractService.Application.Reports.Command;
using Onyx.ContractService.Application.ContractDuration.Queries.GetAuditLog;
using Onyx.ContractService.Application.ContractDuration.Queries.GetCurrencies;
using Onyx.ContractService.Application.ContractDuration.Queries.GetSpecificCurrencies;
using Onyx.ContractService.Application.ContractDuration.Queries.GetAllCurrencies;
using Onyx.ContractService.Application.Contractaudit.Commands.CreateContractaudit;
using Onyx.ContractService.Application.Currencies.Commands.UpdateCurrencyConfiguration;
using Onyx.ContractService.Application.Currencies.Commands.ChangeCurrencyStatus;
using Onyx.ContractService.Application.Currencies.Queries.GetCurrencies;
using Onyx.ContractService.Application.Currencies.Commands.CreateCurrenciesConfiguration;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CurrencyController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public CurrencyController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateCurrencyConfigCommand command)
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
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcurrencies")]
        public async Task<ActionResult<Result>> CreateCurrencies(CreateCurrenciesConfigurationCommand command)
        {
            try
            {
                
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateCurrencyConfigurationCommand command)
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
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecurrencystatus")]
        public async Task<ActionResult<Result>> UpdateCurrencyStatus(ChangeCurrencyStatusCommand command)
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
                return Result.Failure($"Currency Config Creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


        [HttpGet("getallcurrencies/{organisationid}")]
        public async Task<ActionResult<Result>> GetAllCurrencies(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetAllCurrencies { OrganisationId = organisationid ,AccessToken = accessToken});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving all Currencies was not successful" + ex?.Message +" "+ ex?.InnerException?.Message });
            }

        }

        [HttpGet("getcurrencyenums/{organisationid}")]
        public async Task<ActionResult<Result>> GetCurrencyEnums(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetCurrencyEnums ());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving currency enums was not successful" + ex?.Message +" "+ ex?.InnerException?.Message });
            }

        }

        [HttpGet("getactivecurrencies/{organisationid}")]
        public async Task<ActionResult<Result>> GetActiveCurrencies(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetActiveCurrencies { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active currencies was not successful" + ex?.Message +" "+ ex?.InnerException?.Message });
            }

        }

        [HttpGet("getactivecurrenciesobject/{organisationid}")]
        public async Task<ActionResult<Result>> GetActiveCurrenciesObject(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetActiveCurrenciesObject { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active currencies object was not successful" + ex?.Message +" "+ ex?.InnerException?.Message });
            }

        }

        [HttpGet("getcurrencies")]
        public async Task<ActionResult<Result>> GetCurrencies()
        {
            try
            {
                return await Mediator.Send( new GetCurrencies());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving currencies was not successful" + ex?.Message +" "+ ex?.InnerException?.Message });
            }

        }
    }
}