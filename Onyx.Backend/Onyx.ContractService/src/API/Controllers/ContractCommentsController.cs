using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using System;
using Onyx.ContractService.Application.Common.Exceptions;
using System.Threading.Tasks;
using Onyx.ContractService.Application.ContractComments.Commands.CreateContractComment;
using Onyx.ContractService.Application.ContractComments.Commands.UpdateContractComment;
using Onyx.ContractService.Application.ContractComments.Commands.UpdateContractCommentStatus;
using Onyx.ContractService.Application.ContractComments.Queries.GetContractComments;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Authorization;
using Onyx.ContractService.Application.ContractComments.Queries;
using Onyx.ContractService.Application.ContractComments.Commands.DeleteContractComment;
using Onyx.ContractService.Domain.Enums;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractCommentsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractCommentsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractCommentCommand command)
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
                return Result.Failure($"Contract comment creation was not successful. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateContractCommentCommand command)
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
               return Result.Failure($"Contract comment update failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

       
        [HttpPost("updatecontractcommentstatus")]
        public async Task<ActionResult<Result>> UpdateContractCommentStatus(UpdateContractCommentStatusCommand command)
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

                return Result.Failure($"Changing contract comment status was not successful. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontractcommentbyorganisation/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractCommentByOrganisation(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractCommentsByOrgIdQuery { OrganisationId = organisationid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract comment retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpGet("getcontractcommentbycontract/{organisationid}/{contractid}")]
        public async Task<ActionResult<Result>> GetContractCommentByContract(int organisationid, int contractid)
        {
            try
            {
                return await Mediator.Send(new GetContractCommentsByContractQuery { OrganisationId = organisationid , ContractId = contractid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract comment retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpGet("getcontractcommentbyid/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> GetContractCommentById(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new GetContractCommentByIdQuery { OrganisationId = organisationid, Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get contract comment by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try

            {
                return await Mediator.Send(new GetContractCommentsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get contract comments failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpDelete("deletecontractcomment/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> Delete(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteContractCommentCommand { OrganizationId = organisationid, Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Comment Delete failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontractcommentsbycommenttype/{organisationId}/{contractId}/{commentType}")]
        public async Task<ActionResult<Result>> GetContractCommentsByTypeQuery(int organisationid, int contractId, ContractCommentType commentType)
        {
            try
            {
                return await Mediator.Send(new GetContractCommentsByTypeQuery {  OrganisationId = organisationid, ContractId = contractId, ContractCommentType = commentType, AccessToken= accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Getting Contract Comment by comment type failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontractcommentsbycomment/{organisationId}/{contractId}/{email}/{commentType}")]
        public async Task<ActionResult<Result>> GetContractCommentsByEmailAndTypeQuery(int organisationid, int contractId, string email, ContractCommentType commentType)
        {
            try
            {
                return await Mediator.Send(new GetContractCommentsByEmailAndTypeQuery { OrganisationId = organisationid, ContractId = contractId, Email = email, ContractCommentType = commentType, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Getting Contract Comment by comment type and email failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }
}
