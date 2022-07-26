using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Newtonsoft.Json;
using System.Linq;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class UpdatePaystackPaymentCommand : AuthToken, IRequest<Result>
    { 
        public string ReferenceNo { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePaystackPaymentCommandHandler : IRequestHandler<UpdatePaystackPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IPaystackService _paystackService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public UpdatePaystackPaymentCommandHandler(IApplicationDbContext context, IAuthService authService, IConfiguration configuration,
            IPaystackService paystackService, INotificationService notificationService, IEmailService emailService)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
            _paystackService = paystackService;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(UpdatePaystackPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any Payment already exists. If yes, then return a failure response else go ahead and create the Payment
                var payment = await _context.Payments.FirstOrDefaultAsync(a => a.ReferenceNo == request.ReferenceNo && a.Status == Status.Active);

                if (payment != null && payment.PaymentStatus == PaymentStatus.Success)
                {
                    return Result.Failure($"Payment for subscription: {payment.SubscriptionNo} with reference number: {payment.ReferenceNo} has already been processed successfully. Please create a new subscription before proceeding to pay or contact support for more assistance.");
                }

                var respo = await _authService.GetSubscriberAsync(request.AccessToken, payment.SubscriberId, request.UserId);

                if (_authService.Subscriber == null)
                {
                    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                }
                var subscriber = _authService.Subscriber;

                //check for SubscriptionNo and Subscriber Id
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == payment.SubscriberId);

                if (subscription == null)
                {
                    return Result.Failure("Invalid subscription number and subscriber specified!");
                }

                if (payment == null)
                {
                    return Result.Failure("Invalid payment session id specified for verification");
                }

                var totalAmount = subscription.TotalAmount + subscription.TransactionFee;
               
                var response = await _paystackService.VerifyPaymentStatus(payment.ClientReferenceNo);

                if (!response.Status)
                {
                    return Result.Failure("Verifying Paystack Payment with reference number " + payment.ClientReferenceNo + "is " + response.Status + " with message " + response.Message);
                }

                if ((response.Data.Requested_Amount/100) != subscription.TotalAmount)
                {
                    return Result.Failure("Error with subscription Number " + payment.SubscriptionNo + "Amount from Paystack is not equal to subscription amount. Please contact support");
                }

                var message = "";
                switch (response.Data.Status)
                {
                    case "success":
                        payment.PaymentStatus = PaymentStatus.Success;
                        payment.PaymentStatusDesc = PaymentStatus.Success.ToString();
                        subscription.Status = Status.Active;
                        subscription.StatusDesc = Status.Active.ToString();
                        subscription.SubscriptionStatus = SubscriptionStatus.Active;
                        subscription.SubscriptionStatusDesc = SubscriptionStatus.Active.ToString();
                        message = "Your subscription payment was successful";
                        break;
                    case "abandoned":
                        payment.PaymentStatus = PaymentStatus.Cancelled;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You abandoned your subscription payment";
                        break;
                    case "cancelled":
                        payment.PaymentStatus = PaymentStatus.Cancelled;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You cancelled your subscription payment";
                        break;
                    case "failed":
                        payment.PaymentStatus = PaymentStatus.Failed;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You subscription payment failed";
                        break;
                    case "failure":
                        payment.PaymentStatus = PaymentStatus.Failed;
                        subscription.SubscriptionStatus = SubscriptionStatus.Cancelled;
                        subscription.PaymentStatus = PaymentStatus.Failed;
                        message = "Oops! You subscription payment failed";
                        break;
                    case "processing":
                        payment.PaymentStatus = PaymentStatus.Processing;
                        subscription.SubscriptionStatus = SubscriptionStatus.ProcessingPayment;
                        subscription.PaymentStatus = PaymentStatus.Processing;
                        message = "Your subscription payment is processing";
                        break;
                    default:
                        break;
                }

                payment.PaymentStatusDesc = payment.PaymentStatus.ToString();
                subscription.PaymentStatusDesc = subscription.PaymentStatus.ToString();
                subscription.SubscriptionStatusDesc = subscription.SubscriptionStatus.ToString();
                subscription.LastModifiedDate = DateTime.Now;
                subscription.LastModifiedBy = subscriber.Name;
                subscription.PaymentChannelReference = payment.ClientReferenceNo;
                subscription.PaymentChannelResponse = response.Message;

                await _context.BeginTransactionAsync();
                _context.Payments.Update(payment);
                _context.Subscriptions.Update(subscription);

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                await SendPaymentEmail(subscriber, subscription, message);

                return Result.Success("Paystack verification status is: " + payment.PaymentStatus.ToString());
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Paystack verification failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }

        private async Task SendPaymentEmail(SubscriberDto subscriber, Subscription subscription, string message)
        {
            var email = new EmailVm
            {
                Application = _configuration["Email:AppName"],
                Subject = "Subscription Payment",
                RecipientEmail = subscriber.ContactEmail,
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
        }

        //private async Task SendPaymentNotification()
        //{
        //    // await _context.Notifications.AddAsync(notification); To do: Are we including this?
        //    //var notification = new Notification
        //    //{
        //    //    ApplicationType = request.ApplicationType,
        //    //    DeviceId = subscriber.DeviceId,
        //    //    Message = message,
        //    //    NotificationStatus = NotificationStatus.Unread,
        //    //    CreatedBy = subscriber.Name,
        //    //    CreatedDate = DateTime.Now
        //    //};
        //    //notification.SubscriberId = request.SubscriberId;
        //    //await _notificationService.SendNotification(notification.DeviceId, notification.Message);
        //    return;
        //}
    }
}