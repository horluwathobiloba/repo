using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onyx.ContractService.API.Controllers;
using Onyx.ContractService.Application.Common.Models;
using Onyx.ContractService.Application.ContractTags.Commands.CreateContractTag;
using Onyx.ContractService.Application.ContractTags.Queries.GetContractTags;
using System;
using System.Threading.Tasks;
using Onyx.ContractService.Application.Common.Exceptions;
using Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTag;
using Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTagStatus;
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTag;
using Onyx.ContractService.Application.ContractTag.Queries.GetContractTags;
using Onyx.ContractService.Application.ContractTags.Commands.CreateContractTags;
using Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onyx.ContractService.Application.ContractTags.Queries;
using Onyx.ContractService.Application.ContractTags.Commands.DeleteContractTag;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractTagsController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ContractTagsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Result>> Create(CreateContractTagCommand command)
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
                return Result.Failure($"Task tag creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("createcontractags")]
        public async Task<ActionResult<Result>> CreateContractTags(CreateContractTagsCommand command)
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
                return Result.Failure($"Task tags creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<Result>> Update(UpdateContractTagCommand command)
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

                return Result.Failure($"Task tag creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }


        [HttpPost("updatecontracttags")]
        public async Task<ActionResult<Result>> UpdateContractTags(UpdateContractTagsCommand command)
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

                return Result.Failure($"Vendor types creation failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpPost("updatecontracttagstatus")]
        public async Task<ActionResult<Result>> UpdateContractTagStatus(UpdateContractTagStatusCommand command)
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

                return Result.Failure($"Changing task tag status failed. Error: { ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }

        [HttpGet("getcontracttags/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractTags(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractTagQuery { OrganisationId = organisationid });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Task tag retrieval failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttagbyid/{id}")]
        public async Task<ActionResult<Result>> GetContractTagById(int id)
        {
            try
            {
                return await Mediator.Send(new GetContractTagByIdQuery { Id = id, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Get task tag by Id failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttagsbycontract/{organisationid}/{contractid}")]
        public async Task<ActionResult<Result>> GetContractTagsByContract(int organisationid, int contractid)
        {
            try
            {
                return await Mediator.Send(new GetContractTagsByContractQuery { OrganisationId = organisationid, ContractId = contractid , AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract tags retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("getcontracttagsbyorganisation/{organisationid}")]
        public async Task<ActionResult<Result>> GetContractTagsByOrganisationId(int organisationid)
        {
            try
            {
                return await Mediator.Send(new GetContractTagsByOrgIdQuery { OrganisationId = organisationid, AccessToken = accessToken });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Contract tags retrieval was not successful. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
        public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
        {
            try
            {
                return await Mediator.Send(new GetContractTagsDynamicQuery { OrganisationId = organisationid, SearchText = searchtext});
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get task tag by name failed. Eror: {ex?.Message +" "+ ex?.InnerException?.Message}");
            }
        }
        [HttpDelete("deletecontracttag/{organisationid}/{id}")]
        public async Task<ActionResult<Result>> Delete(int organisationid, int id)
        {
            try
            {
                return await Mediator.Send(new DeleteContractTagCommand { OrganizationId = organisationid, Id = id });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message +" "+ ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Task delete failed. Error: {ex?.Message +" "+ ex?.InnerException?.Message }");
            }
        }
    }
}
