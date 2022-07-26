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
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.CreateAddressBook;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBook;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.DeleteAddressBook;
using OnyxDoc.DocumentService.Application.AddressBooks.Queries.GetAddressBooks;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBookStatus;
using OnyxDoc.DocumentService.Application.Recipients.Queries.GetRecipients;

    namespace API.Controllers
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class AddressBooksController : ApiController
        {
            [HttpPost("create")]
            public async Task<ActionResult<Result>> Create(CreateAddressBookCommand command)
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
                    return Result.Failure($"Address Book creation failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

           

            [HttpPost("update")]
            public async Task<ActionResult<Result>> Update(UpdateAddressBookCommand command)
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

                    return Result.Failure($"Address Book update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

            [HttpPost("delete/{subscriberid}/{id}")]
            public async Task<ActionResult<Result>> Delete(int subscriberid, int id)
            {
                try
                {
                    return await Mediator.Send(new DeleteAddressBookCommand { SubscriberId = subscriberid, Id = id });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($"Address Book update failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
                }
            }

          

            [HttpPost("updateaddressbookstatus")]
            public async Task<ActionResult<Result>> UpdateAddressBookStatus(UpdateAddressBookStatusCommand command)
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

                    return Result.Failure($"Changing Address Book status was not successful. Error: { ex?.Message ?? ex?.InnerException?.Message }");
                }
            }



         

            [HttpGet("getaddressbook/{id}")]
            public async Task<ActionResult<Result>> GetAddressBook(int id)
            {
                try
                {
                    return await Mediator.Send(new GetAddressBookByIdQuery {  AddressBookId = id });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {
                    return Result.Failure($"Get Address Book by Id failed. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

           
            [HttpGet("getaddressbookbyemail/{email}")]
            public async Task<ActionResult<Result>> GetAddressBookByEmail(string email)
            {
                try
                {
                    return await Mediator.Send(new GetAddressBookByEmailQuery { Email = email });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($"Get Address Book by email failed. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

            [HttpGet("getaddressbooksbysubscriberid/{subscriberid}")]
            public async Task<ActionResult<Result>> GetAddressBooksBySubscriberId(int subscriberid)
            {
                try
                {
                    return await Mediator.Send(new GetAddressBooksBySubscriberIdQuery { SubscriberId = subscriberid });
                }
                catch (ValidationException ex)
                {
                    return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
                }
                catch (System.Exception ex)
                {

                    return Result.Failure($"Get Address Book by document id failed. Eror: {ex?.Message ?? ex?.InnerException?.Message}");
                }
            }

        }
    }
