using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Linq;
using OnyxDoc.SubscriptionService.Domain.ViewModels;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class CreateStripeCardPaymentCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriptionNo { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CallBackUrl { get; set; }
        public ApplicationType LoggedInDevice { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public string SessionId { get; set; }

        [JsonIgnore]
        public string PaymentMethodType { get; set; }

        [JsonIgnore]
        public string SuccessUrl { get; set; }

        [JsonIgnore]
        public string CancelUrl { get; set; }

        [JsonIgnore]
        public long Quantity => 1;

        [JsonIgnore]
        public string Mode => "Payment";

        [JsonIgnore]
        public string Description => $"Payment for Subscription No: {this.SubscriptionNo}";

    }

    public class CreateStripeCardPaymentCommandHandler : IRequestHandler<CreateStripeCardPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IStripeService _stripeService;

        public CreateStripeCardPaymentCommandHandler(IApplicationDbContext context, IConfiguration configuration, IStripeService stripeService, IAuthService authService)
        {
            _context = context;
            _configuration = configuration;
            _stripeService = stripeService;
            _authService = authService;
        }

        public async Task<Result> Handle(CreateStripeCardPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CurrencyCode == CurrencyCode.NGN)
                {
                    return Result.Failure("Invalid Currency Code for stripe payment");
                }

                //check for SubscriptionNo and Subscriber Id
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionNo == request.SubscriptionNo);
                if (subscription == null)
                {
                    return Result.Failure("Invalid Subscription Number and Subscriber Id ");
                }
                //get customer object 
                var subscriber = (await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId)).Entity;

                var finalSubscriptionAmount = subscription.TotalAmount + subscription.TransactionFee;

                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    UnitAmount = (long)finalSubscriptionAmount * 100,//kobo equivalent
                    CancelUrl = request.CancelUrl,
                    CurrencyCode = request.CurrencyCode.ToString(),
                    SubscriberId = request.SubscriberId,
                    CallBackUrl = request.CallBackUrl,
                    TransactionFee = subscription.TransactionFee,
                    SubscriptionNo = request.SubscriptionNo,
                    SessionId = request.SessionId,
                    Email = subscriber?.Email
                };

                var result = await _stripeService.InitiateCardPayment(paymentIntentVm);
                var sessionId = result.Entity.ToString();

                //check if any Payment already exists.. If yes, then return a failure response else go ahead and create the Payment
                var exists = await _context.Payments.AnyAsync(a => a.SessionId == sessionId && a.SubscriptionNo.ToUpper() == request.SubscriptionNo.ToUpper()
                && (a.PaymentStatus == PaymentStatus.Processing || a.PaymentStatus == PaymentStatus.Success) && a.Status == Status.Active);

                if (exists)
                {
                    return Result.Failure(new string[] { $"Create new Payment failed because a payment with the same session id: {request.SessionId} and subscription no: {request.SubscriptionNo} already exists. Please create a new subscription before proceeding to pay or contact support for more assistance." });
                }

                var entity = new Payment
                {
                    SubscriberId = request.SubscriberId,
                    SessionId = sessionId,
                    Amount = finalSubscriptionAmount,
                    Description = request.Description,
                    CurrencyCode = request.CurrencyCode.ToString(),
                    Quantity = request.Quantity,
                    Mode = request.Mode,
                    PaymentMethodType = request.PaymentMethodType,
                    SubscriptionNo = request.SubscriptionNo,
                    TransactionFee = subscription.TransactionFee,
                    SuccessUrl = request.SuccessUrl,
                    CancelUrl = request.CancelUrl,
                    CreatedBy = subscriber.Email,
                    PaymentStatus = PaymentStatus.Initiated,
                    PaymentStatusDesc = PaymentStatus.Initiated.ToString(),
                    LogDate = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    ClientReferenceNo = string.Format("{0:ddMMyyyyHHmmssfff}-{1}", DateTime.Now, request.SubscriptionNo),
                    SubscriptionId = subscription.Id
                };

                _context.Payments.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                /*return Result.Success("Payment initiated successfully!", entity.SessionId);*/
                return Result.Success("Payment initiated successfully!", entity);

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment initiation failed!", ex?.Message ?? ex?.InnerException.Message
    });
            }
        }
    }
}