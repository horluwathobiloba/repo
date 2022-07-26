using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Models.Paystack;

namespace OnyxDoc.SubscriptionService.Application.Payments.WebHooks
{
    public class CreatePaystackWebHookEventCommand : IRequest<Result>
    {
        public string Result { get; set; }
    }
    public class CreatePaystackWebHookEventCommandHandler : IRequestHandler<CreatePaystackWebHookEventCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public CreatePaystackWebHookEventCommandHandler(IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Result> Handle(CreatePaystackWebHookEventCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var result = JsonConvert.DeserializeObject<PaystackPaymentResponseRaw>(request.Result);
                //check if any Payment already exists.. If yes, then return a failure response else go ahead and create the Payment
                var existingPayment = await _context.Payments.FirstOrDefaultAsync(a => a.SessionId == result.Data.Access_Code
                 && a.Status == Status.Active);
                if (existingPayment != null && existingPayment.PaymentStatus == PaymentStatus.Success)
                {
                    return Result.Failure(new string[] { $"Update new Payment failed because a payment with the same payment intent id: {result.Data.Access_Code}  already exists. Please create a new subscription before proceeding to pay or contact support for more assistance." });
                }

                switch (result.Data.Status)
                {
                    case "success":
                        existingPayment.PaymentStatus = PaymentStatus.Success;
                        break;
                    case "abandoned":
                        existingPayment.PaymentStatus = PaymentStatus.Cancelled;
                        break;
                    case "cancelled":
                        existingPayment.PaymentStatus = PaymentStatus.Cancelled;
                        break;
                    case "failed":
                        existingPayment.PaymentStatus = PaymentStatus.Failed;
                        break;
                    case "failure":
                        existingPayment.PaymentStatus = PaymentStatus.Failed;
                        break;
                    case "processing":
                        existingPayment.PaymentStatus = PaymentStatus.Processing;
                        break;
                    default:
                        break;
                }
                _context.Payments.Update(existingPayment);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Paystack Payment verification is " + existingPayment.Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private PaymentStatus GetPaymentStatus(string PaystackStatus)
        {
            switch (PaystackStatus)
            {
                case "succeeded":
                    return PaymentStatus.Success;

                case "canceled":
                    return PaymentStatus.Cancelled;

                case "processing":
                    return PaymentStatus.Processing;

                case "requires_action":
                    return PaymentStatus.RequiresAction;

                case "requires_capture":
                    return PaymentStatus.RequiresCapture;

                case "requires_confirmation":
                    return PaymentStatus.RequiresConfirmation;

                case "requires_payment_method":
                    return PaymentStatus.RequiresPaymentMethod;

                default:
                    return PaymentStatus.Processing;
            }
        }
    }
}


