using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipients;
using Onyx.ContractService.Application.ContractRecipients.Commands.CreateContractRecipients;
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipient;
using Onyx.ContractService.Application.ContractRecipients.Queries.GetContractRecipients;  
using Onyx.ContractService.Application.ContractRecipients.Commands.CreateContractRecipient;
using Onyx.ContractService.Application.ContractRecipients.Commands.DeleteContractRecipient; 
using Onyx.ContractService.Application.ContractRecipients.Commands.UpdateContractRecipientStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractRecipientsController : ApiController
    {
        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractRecipientCommand command)
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
                return Result.Failure($"Contract recipient creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcontractrecipients")]
        public async Task<ActionResult<Result>> CreateContractRecipients(CreateContractRecipientsCommand command)
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
                return Result.Failure($"Contract recipient creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateContractRecipientCommand command)
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

                return Result.Failure($"Contract recipient update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("delete/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> Delete( int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteContractRecipientCommand { OrganisationId = organisationid, Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract recipient update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontractrecipients")]
        public async Task<ActionResult<Result>> UpdateContractRecipients(UpdateContractRecipientsCommand command)
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

                return Result.Failure($"Contract recipients update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontractrecipientstatus")]
        public async Task<ActionResult<Result>> UpdateContractRecipientstatus(UpdateContractRecipientStatusCommand command)
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

                return Result.Failure($"Changing Contract recipient status was not successful. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        

       [HttpGet("getcontractrecipients/{organisationid}/{contractId}")]
        public async Task<ActionResult<Result>> GetContractRecipientsByContract(int organisationid, int contractId)
        {
            try
            {
                return await Mediator.Send(new GetContractRecipientsByContractQuery { OrganisationId = organisationid , ContractId = contractId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract recipients retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractrecipients/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractRecipients(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractRecipientsQuery { OrganisationId = organisationid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract recipients retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractsignatories/{organisationid}/{contractid}")]
        public async Task<ActionResult<Result>> GetContractSignatories(int organisationid, int contractid)
        {
            try
            {
                return await Mediator.Send(new GetContractSignatoryByContractQuery { OrganisationId = organisationid , ContractId = contractid});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract signatories retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractrecipient/{id}")]
        public async Task<ActionResult<Result>> GetContractRecipient(int id)
        {
            try
            {
                return await Mediator.Send(new GetContractRecipientQuery { Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Contract recipient by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractrecipientsbycontract/{organisationid}/{contractid}")]
        public async Task<ActionResult<Result>> GetContractRecipientsByPhase(int organisationid, int contractid)
        {
            try
            {
                return await Mediator.Send(new GetContractRecipientsByContractQuery { OrganisationId = organisationid, ContractId = contractid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contract recipients by phase failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpGet("getcontractrecipientsbyaction/{organisationid}/{email}")]
        public async Task<ActionResult<Result>> GetContractRecipientsByEmail(int organisationid, string email)
        {
            try
            {
                return await Mediator.Send(new GetContractRecipientsByRoleQuery { OrganisationId = organisationid,  Email = email });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contract recipients by email failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetContractRecipientsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contract recipients. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

    }
}
