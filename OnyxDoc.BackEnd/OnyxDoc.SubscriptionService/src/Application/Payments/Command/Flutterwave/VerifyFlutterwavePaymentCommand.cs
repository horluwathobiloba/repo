using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class VerifyFlutterwavePaymentCommand : AuthToken, IRequest<Result>
    {
        public string Hash { get; set; }
        public string TransactionId { get; set; }
    }

    public class VerifyFlutterwavePaymentCommandHandler : IRequestHandler<VerifyFlutterwavePaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IFlutterwaveService _flutterwaveService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public VerifyFlutterwavePaymentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                      IFlutterwaveService flutterwaveService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _flutterwaveService = flutterwaveService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(VerifyFlutterwavePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {

                //check if any Payment already exists. If yes, then return a failure response else go ahead and create the Payment
                var hash = HttpUtility.UrlEncode(request.Hash);
               
                var existingPayment = await _context.Payments.FirstOrDefaultAsync(a => a.Hash == hash && a.Status == Status.Active);
                if (existingPayment == null)
                {
                    return Result.Failure("Invalid Payment Details Specified");
                }
                if (existingPayment != null && existingPayment.PaymentStatus == PaymentStatus.Success && existingPayment.SessionId == request.TransactionId)
                {
                    return Result.Failure($"Payment for subscription: {existingPayment.SubscriptionNo} with transaction id : {request.TransactionId} has already been processed successfully. Please create a new subscription before proceeding to pay or contact support for more assistance.");
                }

                var respo = await _authService.GetSubscriberAsync(request.AccessToken, existingPayment.SubscriberId, existingPayment.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var subscriber = _authService.Subscriber;

                //check for SubscriptionNo and Subscriber Id
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == existingPayment.SubscriberId && a.SubscriptionNo == existingPayment.SubscriptionNo && a.SubscriberId == existingPayment.SubscriberId);
                if (subscription == null)
                {
                    return Result.Failure("Invalid subscription number and subscriber specified!");
                }

                if (existingPayment == null)
                {
                    return Result.Failure("Invalid payment session id specified for verification");
                }

              
                var verifyResponse =  await _flutterwaveService.GetPaymentStatus(request.TransactionId);
                if (verifyResponse.status != "success")
                {
                    return Result.Failure("Verifying Flutterwave Payment with reference number " + existingPayment.ClientReferenceNo + "is " + verifyResponse.status + " with message " + verifyResponse.message);
                }

                if (Convert.ToDecimal(verifyResponse.data.amount) != subscription.TotalAmount)
                {
                    return Result.Failure("Error with subscription Number " + existingPayment.SubscriptionNo + "Amount from flutterwave is not equal to subscription amount. Please contact support");
                }

                var message = "";
                switch (verifyResponse.status)
                {
                    case "success":
                        existingPayment.PaymentStatus = PaymentStatus.Success;
                        existingPayment.PaymentStatusDesc = PaymentStatus.Success.ToString();
                        subscription.Status = Status.Active;
                        subscription.StatusDesc = Status.Active.ToString();
                        subscription.SubscriptionStatus = SubscriptionStatus.Active;
                        subscription.SubscriptionStatusDesc = SubscriptionStatus.Active.ToString();
                        message = "Your subscription payment has been fulfilled successfully";
                        break;
                    case "abandoned":
                        existingPayment.PaymentStatus = PaymentStatus.Cancelled;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You abandoned your subscription payment";
                        break;
                    case "cancelled":
                        existingPayment.PaymentStatus = PaymentStatus.Cancelled;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You cancelled your subscription payment";
                        break;
                    case "failed":
                        existingPayment.PaymentStatus = PaymentStatus.Failed;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You subscription payment failed";
                        break;
                    case "failure":
                        existingPayment.PaymentStatus = PaymentStatus.Failed;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You subscription payment failed";
                        break;
                    case "processing":
                        existingPayment.PaymentStatus = PaymentStatus.Processing;
                        subscription.SubscriptionStatus = SubscriptionStatus.ProcessingPayment;
                        subscription.PaymentStatus = PaymentStatus.Processing;
                        message = "Your subscription payment is processing";
                        break;
                    default:
                        break;
                }

                existingPayment.PaymentStatusDesc = existingPayment.PaymentStatus.ToString();
                subscription.PaymentStatusDesc = subscription.PaymentStatus.ToString();
                subscription.SubscriptionStatusDesc = subscription.SubscriptionStatus.ToString();
                subscription.LastModifiedDate = DateTime.Now;
                subscription.LastModifiedBy = subscription.SubscriberName;
                subscription.PaymentChannelReference = existingPayment.ClientReferenceNo;
                subscription.PaymentChannelResponse = verifyResponse.message;

               
                await _context.BeginTransactionAsync();

                _context.Payments.Update(existingPayment);
                _context.Subscriptions.Update(subscription);
                // await _context.Notifications.AddAsync(notification); To do: Are we including this?

                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();

                var email = new EmailVm
                {
                    Application = _configuration["Email:AppName"],
                    Subject = "Subscription Payment",
                    RecipientEmail = subscriber.Email,
                    RecipientName = subscriber.Name,
                    Text = subscription.SubscriptionStatus == SubscriptionStatus.Active ? "Hurray!" : "",
                    Body1 = message,
                    Body2 = "",
                    Body3 = "",
                    ButtonText = "Subscription Payment",
                    ImageSource = _configuration["SVG:PaymentReceived"],
                    DisplayButton = "display:none;"
                };
                var img = _configuration["SVG:EmailVerification"];
                 await _emailService.SendEmail(email);

                return Result.Success(verifyResponse);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Verify Flutterwave Payment failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
