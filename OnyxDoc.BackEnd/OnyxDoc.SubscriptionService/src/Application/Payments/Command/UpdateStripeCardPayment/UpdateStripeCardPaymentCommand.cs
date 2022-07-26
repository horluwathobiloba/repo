using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using ReventInject;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using OnyxDoc.SubscriptionService.Domain.Entities;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class UpdateStripeCardPaymentCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriptionNo { get; set; }
        public string SessionId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateStripeCardPaymentCommandHandler : IRequestHandler<UpdateStripeCardPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IPaystackService _paystackService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly IStripeService _stripeService;

        public UpdateStripeCardPaymentCommandHandler(IApplicationDbContext context, IAuthService authService, IConfiguration configuration,
          IPaystackService paystackService, INotificationService notificationService, IEmailService emailService)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
            _paystackService = paystackService;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(UpdateStripeCardPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any successful Payment already exists.. If yes, then return a failure response else go ahead and update the Payment
                var existingPayment = await _context.Payments.FirstOrDefaultAsync(a => a.SubscriptionNo.ToUpper() == request.SubscriptionNo.ToUpper() && a.SubscriberId == request.SubscriberId && a.Status == Status.Active);

                if (existingPayment != null && existingPayment.PaymentStatus == PaymentStatus.Success && existingPayment.SessionId == request.SessionId)
                {
                    return Result.Failure($"Payment for subscription: {request.SubscriptionNo} with sessionid: {request.SessionId} has already been processed successfully. Please create a new subscription before proceeding to pay or contact support for more assistance.");
                }
                if (existingPayment == null)
                {
                    return Result.Failure("Invalid payment session id specified for verification!");
                }

                //check for SubscriptionNo and Subscriber Id
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionNo == request.SubscriptionNo && a.SubscriberId == request.SubscriberId);

                var subscriber = (await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId))?.Entity;

                if (subscription == null)
                {
                    return Result.Failure("Invalid subscription number and subscriber specified!");
                }

                var subscriptionAmount = subscription.TotalAmount + subscription.TransactionFee;
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    SubscriberId = request.SubscriberId,
                    SubscriptionNo = request.SubscriptionNo,
                    SessionId = request.SessionId,
                    ClientReferenceNo = existingPayment.ClientReferenceNo,
                    SubscriptionAmount = (long)subscriptionAmount * 100 //kobo/cent equivalent

                };

                var result = await _stripeService.GetPaymentStatus(paymentIntentVm);
                //get payment status using sessionID
                if (result.Entity == null)
                {
                    return Result.Failure("Error retrieving session value from Stripe");
                }
                var session = result.Entity.CastTo<Session>();
                if (session.AmountTotal.Value != paymentIntentVm.UnitAmount)
                {
                    return Result.Failure("Error with Subscription Number " + request.SubscriptionNo + "Amount from stripe is not equal to subscription amount. Please contact support");
                }

                var message = "";
                switch (session.PaymentStatus)
                {
                    case "paid":
                        if (session.PaymentStatus == "paid")
                        {
                            existingPayment.PaymentStatus = PaymentStatus.Success;
                            existingPayment.PaymentStatusDesc = PaymentStatus.Success.ToString();
                            subscription.Status = Status.Active;
                            subscription.StatusDesc = Status.Active.ToString();
                            subscription.SubscriptionStatus = SubscriptionStatus.Active;
                            subscription.SubscriptionStatusDesc = SubscriptionStatus.Active.ToString();
                            message = "Your subscription payment was successful";
                        }
                        break;
                    case "unpaid":
                        if (session.PaymentStatus == "unpaid")
                        {
                            existingPayment.PaymentStatus = PaymentStatus.Failed;
                            subscription.PaymentStatus = PaymentStatus.Failed;
                            subscription.SubscriptionStatus = SubscriptionStatus.ProcessingPayment;
                            message = "Oops! You subscription payment failed";
                        }
                        break;
                }

                existingPayment.PaymentStatusDesc = existingPayment.PaymentStatus.ToString();
                subscription.PaymentStatusDesc = subscription.PaymentStatus.ToString();

                subscription.PaymentChannelReference = paymentIntentVm.ClientReferenceNo;
                subscription.PaymentChannelResponse = result.Message;
                subscription.LastModifiedDate = DateTime.Now;
                subscription.LastModifiedBy = subscriber.Email;
                subscription.SubscriptionStatusDesc = subscription.SubscriptionStatus.ToString();

                _context.Payments.Update(existingPayment);
                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync(cancellationToken);

                var notification = new Notification
                {
                    ApplicationType = request.ApplicationType,
                    DeviceId = subscriber.DeviceId,
                    Message = message,
                    NotificationStatus = NotificationStatus.Unread,
                    CreatedBy = subscriber.Name,
                    CreatedDate = DateTime.Now
                };
                notification.SubscriberId = request.SubscriberId;
                await _notificationService.SendNotification(notification.DeviceId, notification.Message);

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
                    ButtonText = "Subcription Payment",
                    ImageSource = _configuration["SVG:PaymentReceived"],
                    DisplayButton = "display:none;"
                };
                var img = _configuration["SVG:EmailVerification"];
                var emailResp = await _emailService.SendEmail(email);


                return Result.Success("Stripe payment is " + existingPayment.PaymentStatus.ToString());

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Unable to retrieve status of stripe payment!", ex?.Message ?? ex?.InnerException.Message
    });
            }
        }
    }
}