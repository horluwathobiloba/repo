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
using OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipient;
using OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipients;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipient;
using OnyxDoc.DocumentService.Application.Recipients.Commands.DeleteRecipient;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;
using OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipientStatus;
using OnyxDoc.DocumentService.Application.Recipient.Queries.GetRecipient;
using OnyxDoc.DocumentService.Application.Recipients.Commands.UpdateRecipients;

namespace API.Controllers
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class RecipientsController : ApiController
        {
            [HttpPost("create")]
            public async Task<ActionResult<Result>> Create(CreateRecipientCommand command)
            {
                try
                {
                    return await Mediator.Send(command);
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {
                    return Result.Failure($" recipient creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

            [HttpPost("createrecipients")]
            public async Task<ActionResult<Result>> CreateRecipients(CreateRecipientsCommand command)
            {
                try
                {
                    return await Mediator.Send(command);
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {
                    return Result.Failure($" recipient creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

            [HttpPost("update")]
            public async Task<ActionResult<Result>> Update(UpdateRecipientCommand command)
            {
                try
                {
                    return await Mediator.Send(command);
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($" recipient update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

            [HttpPost("updaterecipients")]
            public async Task<ActionResult<Result>> UpdateRecepients(UpdateRecipientsCommand command)
            {
                try
                {
                    return await Mediator.Send(command);
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch(Exception ex)
                {
                return Result.Failure($" recipients update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

            [HttpPost("delete/{organisationid}/{id}")]
            public async Task<ActionResult<Result>> Delete(int organisationid, int id)
            {
                try
                {
                    return await Mediator.Send(new DeleteRecipientCommand { SubscriberId = organisationid, Id = id });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($" recipient update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

          

            [HttpPost("updaterecipientstatus")]
            public async Task<ActionResult<Result>> UpdateRecipientstatus(UpdateRecipientStatusCommand command)
            {
                try
                {
                    return await Mediator.Send(command);
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($"Changing  recipient status was not successful. Error: { ex?.Message ?? ex?.InnerException?.Message }");
                }
            }




            [HttpGet("getrecipients/{organisationid}")]
            public async Task<ActionResult<Result>> GetRecipients(int organisationid)
            {
                try
                {
                    return await Mediator.Send(new GetRecipientsQuery { SubscriberId = organisationid });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($" recipients retrieval was not successful. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

         

            [HttpGet("getrecipient/{id}")]
            public async Task<ActionResult<Result>> GetRecipient(int id)
            {
                try
                {
                    return await Mediator.Send(new GetRecipientQuery { Id = id });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {
                    return Result.Failure($"Get  recipient by Id failed. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

           
            [HttpGet("getrecipientbyemail/{email}")]
            public async Task<ActionResult<Result>> GetRecipientByEmail( string email)
            {
                try
                {
                    return await Mediator.Send(new GetRecipientByEmailQuery { Email = email });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($"Get recipient by email failed. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

        [HttpGet("getrecipientsbydocumentid/{documentid}")]
        public async Task<ActionResult<Result>> GetRecipientsByDocumentId(int documentId)
        {
            try
            {
                return await Mediator.Send(new GetRecipientsByDocumentIdQuery { DocumentId = documentId });
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {

                return Result.Failure($"Get recipient by document id failed. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        [HttpGet("dynamicsearch/{organisationid}/{searchtext}")]
            public async Task<ActionResult<Result>> DynamicSearch(int organisationid, string searchtext)
            {
                try
                {
                    return await Mediator.Send(new GetRecipientsDynamicQuery { SubscriberId = organisationid, SearchText = searchtext });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($"Get recipients. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

        }
    }
