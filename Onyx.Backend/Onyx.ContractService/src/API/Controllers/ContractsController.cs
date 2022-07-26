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
using Onyx.ContractService.Application.Contracts.Commands.TerminateContract;
using Onyx.ContractService.Application.Contracts.Queries.GetRenewedContract;
using Onyx.ContractService.Application.Contracts.Command.ContractEmailNotification;
using Onyx.ContractService.Application.Contracts.Commands.ApprovalEmailNotification;
using Onyx.ContractService.Application.Contracts.Command.SetExpiredContract;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractCommand command)
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
                return Result.Failure($"Contract creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontract")]
        public async Task<ActionResult<Result>> UpdateContract(UpdateContractCommand command)
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
                return Result.Failure($"Contract update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("submitforapproval")]
        public async Task<ActionResult<Result>> SubmitForApproval(SubmitForApprovalActionCommand command)
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
                return Result.Failure($"Submit for approval failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("terminatecontract")]
        public async Task<ActionResult<Result>> TerminateContract(TerminateContractCommand command)
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

                return Result.Failure($"Terminate contract failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatestatus")]
        public async Task<ActionResult<Result>> UpdateContractStatus(UpdateStatusCommand command)
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

                return Result.Failure($"Update Contract status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontractstatus")]
        public async Task<ActionResult<Result>> UpdateContractStatus(UpdateContractStatusCommand command)
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

                return Result.Failure($"Update Contract status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("setContractStatustoEpired")]
        public async Task<ActionResult<Result>> SetContractStatustoEpired(SetExpiredContractCommand command)
        {
            try
            {
                command.AccessToken = accessToken;
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message + " " + ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Update Contract status yo expired failed. Error: { ex?.Message + " " + ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getrenewedcontract/{organisationid}/{id}/{userid}")]
        public async Task<ActionResult<Result>> GetRenewedContract(int organisationid, int id, string userid)
        {
            try
            {
                return await Mediator.Send(new GetRenewedContractCommand { OrganisationId = organisationid, Id = id, UserId = userid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Terminate contract failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontracthistory/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetContractHistory(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetContractHistoryQuery { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get contract history by the current contract Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracthistorybyinitialid/{organisationid}/{initialcontractid}")]
        public async Task<ActionResult<Result>> GetContractHistoryByInitialId(int organisationid, int initialcontractid)
        {
            try
            {
                return await Mediator.Send(new GetContractHistoryByInitialIdQuery { OrganisationId = organisationid, InitialContractId = initialcontractid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get contract history by the current contract Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }


        [HttpGet("getcontracts/{organisationid}")]
        public async Task<ActionResult<Result>> GetContracts(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract type Initiator retrieval was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontract/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetContract(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetContractQuery { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Contract type Initiator by Id failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractsbycontractstatus/{organisationid}/{contractstatus}")]
        public async Task<ActionResult<Result>> GetContractsByContractStatus(int organisationid, ContractStatus contractstatus)
        {
            try
            {
                return await Mediator.Send(new GetContractsByContractStatusQuery { OrganisationId = organisationid, ContractStatus = contractstatus, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Contracts by contractstatus failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractsbyexpirydate/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractsByExpiryDate(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractsByExpirationDateQuery() { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Contracts by expiration date failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractrequests/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractRequest(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractRequestsQuery() { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Contracts requests date failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getallexecutedcontracts/{organisationid}")]
        public async Task<ActionResult<Result>> GetExecutedContractRequest(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetAllExecutedContractsQuery() { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get all executed Contractsfailed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetContractsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contracts failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getdocumenttypedistribution/{organisationid}/{documenttype}")]
        public async Task<ActionResult<Result>> GetDocumentTypeDistribution(int organisationid, int documenttype)
        {
            try
            {
                return await Mediator.Send(new DocumentDistributionQuery { OrganizationId = organisationid, DocumentType = (DocumentType)documenttype });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Dashboard failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpGet("getdocumentstatquery/{organisationid}/{year}")]
        public async Task<ActionResult<Result>> GetDocumentStat(int organisationid, int year)
        {
            try
            {
                return await Mediator.Send(new GetDocumentStatQuery { OrganizationId = organisationid, Year = year });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Dashboard failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontractworkflowdashboard/{organisationid}/{workflowDate}")]
        public async Task<ActionResult<Result>> GetContractWorkFlowDashboard(int organisationid, DateTime workflowDate)
        {
            try
            {
                return await Mediator.Send(new GetDashBoardQuery { OrganizationId = organisationid, WorkflowDate = workflowDate, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get Dashboard failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }



        #region Executed Contracts
        [HttpPost("createexecutedcontract")]
        public async Task<ActionResult<Result>> CreateExecutedContract(CreateExecutedContractCommand command)
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
                return Result.Failure($"Executed contract creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updateexecutedcontract")]
        public async Task<ActionResult<Result>> UpdateExecutedContract(UpdateExecutedContractCommand command)
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
                return Result.Failure($"Contract update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getexecutedcontracts/{organisationid}")]
        public async Task<ActionResult<Result>> GetExecutedContracts(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetExecutedContractsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get executed contract failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getexecutedcontract/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetExecutedContract(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new Onyx.ContractService.Application.Contracts.Queries.GetExecutedContracts.GetExecutedContractQuery { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get executed contract failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("executedcontractsdynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> ExecutedContractsDynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetExecutedContractsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Contracts failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpGet("getexecutedpermits/{organisationid}")]
        public async Task<ActionResult<Result>> GetExecutedPermits(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetExecutedPermitsQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get executed permit failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getexecutedpermit/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetExecutedPermit(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetExecutedPermitQuery { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get executed contract failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("executedpermitsdynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> ExecutedPermitsDynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetExecutedPermitsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get Executed Permits failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpPost("sendapprovalreminderemails")]
        public async Task<ActionResult<Result>> SendApprovalReminderEmails()
        {
            try
            {
                return await Mediator.Send(new ApprovalEmailNotificationCommand());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Approval Email Notification failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpPost("sendcontractexpiryemails")]
        public async Task<ActionResult<Result>> SendContractExpiryEmails()
        {
            try
            {
                return await Mediator.Send(new ContractEmailNotificationCommand());
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Expiry Contract Email Notification failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        //[HttpPost("sendexpiryemails")]
        //public async Task<ActionResult<Result>> SendExpiryEmails()
        //{
        //    try
        //    {
        //        return await Mediator.Send(new SendExpiredContractEmailCommand());
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Result.Failure($"Contract Email Notification failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
        //    }
        //}
        #endregion

    }
}
