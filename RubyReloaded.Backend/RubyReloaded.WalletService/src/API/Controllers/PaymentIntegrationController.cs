using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Exceptions;
using RubyReloaded.WalletService.Infrastructure.Utility;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Commands;

namespace RubyReloaded.WalletService.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentIntegrationController : ApiController
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentIntegrationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

           accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().ExtractToken();
            if (accessToken == null)
            {
                throw new Exception("You are not authorized!");
            }
        }

        [HttpPost("initializeflutterwavepayment")]
        public async Task<ActionResult<Result>> InitializeFlutterwavePayment(InitiliazeFlutterwavePaymentCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Initiliaze Flutterwave Payment failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("verifyflutterwavepayment")]
        public async Task<ActionResult<Result>> VerifyFlutterwavePayment(VerifyFlutterwavePaymentCommand command)
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
                return Result.Failure($"Verify Flutterwave Payment failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }

        [HttpPost("initializepaystackpayment")]
        public async Task<ActionResult<Result>> InitializePaystackPayment(InitiliazePaystackPaymentCommand command)
        {
            try
            {
                accessToken.ValidateToken(command.UserId);
                accessToken.ValidateBVN();
                return await Mediator.Send(command);
            }
            catch (ValidationException ex)
            {
                return Result.Failure($"{ex?.Message ?? ex?.InnerException?.Message}. Error: {ex.GetErrors()}");
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Initiliaze Paystack Payment failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }
        [HttpPost("verifypaystackpayment")]
        public async Task<ActionResult<Result>> VerifyPaystackPayment(VerifyPaystackPaymentCommand command)
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
                return Result.Failure($"Verify Payment Command failed. Error: {ex?.Message ?? ex?.InnerException?.Message }");
            }
        }


        [HttpGet("getallprovidusbanks")]
        public async Task<ActionResult<Result>> GetAllProvidusBanks()
        {
            try
            {
                return await Mediator.Send(new GetAllProvidusBanksQuery());
            }
            catch (System.Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Bank List from Providus was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }


    }
}