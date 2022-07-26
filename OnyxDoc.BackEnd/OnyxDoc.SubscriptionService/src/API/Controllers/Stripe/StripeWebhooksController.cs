using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Stripe;
using OnyxDoc.SubscriptionService.API.Controllers;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Payments.Commands.UpdatePayment;
using OnyxDoc.SubscriptionService.Application.Payments.Commands.WebHooks;
using OnyxDoc.SubscriptionService.Application.PaymentResponses.Commands.CreatePaymentResponse;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace API.Controllers
{
    public class StripeWebhooksController : ApiController
    {
        private readonly IConfiguration _configuration;

        public StripeWebhooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Index()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                if (string.IsNullOrEmpty(json))
                {
                    return BadRequest("Request is empty");
                }

                var stripeEvent = EventUtility.ParseEvent(json);
                //get payment response status
                var paymentCommand = await Mediator.Send(new CreateStripeWebHookEventCommand {  StripeEvent = stripeEvent});
                await Mediator.Send(paymentCommand.Item1);
                //start updating the payment response
                await Mediator.Send(paymentCommand.Item3);
                return Result.Success(paymentCommand.Item1.PaymentStatus);
            }
            catch (StripeException ex)
            {
                return BadRequest(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
