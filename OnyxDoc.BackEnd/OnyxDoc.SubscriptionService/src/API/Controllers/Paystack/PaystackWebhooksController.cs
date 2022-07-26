using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Paystack.Net.SDK.Transactions;
using OnyxDoc.SubscriptionService.Application.Payments.WebHooks;

namespace OnyxDoc.SubscriptionService.API.Controllers.PayStack
{  
    public class PaystackWebhooksController : ApiController
    {
        private readonly IConfiguration _configuration;

        public PaystackWebhooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return BadRequest("Request is empty");
                }
                string inputString = Convert.ToString(new JValue(json));
                string secretKey = _configuration["Paystack:Key"];
                var paystackTransactionAPI = new PaystackTransaction(secretKey);
                string result = "";
                byte[] secretkeyBytes = Encoding.UTF8.GetBytes(secretKey);
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
                using (var hmac = new HMACSHA512(secretkeyBytes))
                {
                    byte[] hashValue = hmac.ComputeHash(inputBytes);
                    result = BitConverter.ToString(hashValue).Replace("-", string.Empty); ;
                }
                 string xpaystackSignature = HttpContext.Request.Headers["x-paystack-signature"];
                 if (result.ToLower().Equals(xpaystackSignature))
                {
                    var paymentCommand = await Mediator.Send(new CreatePaystackWebHookEventCommand { Result = result });
                }
                else
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message ?? ex?.InnerException?.Message);
            }
        }


    }
}
