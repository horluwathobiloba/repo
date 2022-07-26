using Microsoft.AspNetCore.Mvc;
using OnyxDoc.DocumentService.API.Controllers;
using OnyxDoc.DocumentService.Application.Common.Exceptions;
using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Application.ContactUsMessage.Commands.CreateContactUs;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ContactUsController : ApiController
    {
        [HttpPost("contactusmessage")]
        public async Task<ActionResult<Result>> ContactUsMessage(CreateContactUsCommand command)
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
                return Result.Failure($"Contact Us Feedback failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
    }
}
