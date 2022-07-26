using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Payments.Command.UploadReceipt
{
    public partial class UploadReceiptCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriptionNo { get; set; }
        public string PaymentReceipt { get; set; }
        public int PaymentChannelId { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string UserId { get; set; }
    }

    public class UploadReceiptCommandHandler : IRequestHandler<UploadReceiptCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IBase64ToFileConverter _fileConverter;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UploadReceiptCommandHandler(IApplicationDbContext context, IConfiguration configuration, IAuthService authService, 
            IEmailService emailService,IBase64ToFileConverter fileConverter)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
            _fileConverter = fileConverter;
            _emailService = emailService;
        }

        public async Task<Result> Handle(UploadReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            { 

           
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionNo == request.SubscriptionNo);
            if (subscription == null)
            {
                return Result.Failure("Invalid subscriber or subscription number specified.");
            }

            SubscriberDto subscriber = (await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId)).Entity;
            var paymentChannel = await _context.PaymentChannels.FirstOrDefaultAsync(a => a.CurrencyCode == request.CurrencyCode && a.Id == request.PaymentChannelId);

            var totalAmount = subscription.Amount + paymentChannel.TransactionFee;

            //check if any Payment already exists.. If yes, then return a failure response else go ahead and create the Payment
            var exists = await _context.Payments.AnyAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionNo == request.SubscriptionNo);
            if (exists)
            {
                return Result.Failure(new string[] { $"Create new Payment failed because a payment with the same subscription no: {request.SubscriptionNo} already exists. Please create a new subscription before proceeding to pay or contact support for more assistance." });
            }

                if (request.PaymentReceipt == null)
                {
                    return Result.Failure($"Invalid Payment Receipt");
                }
                subscription.UploadedReceipt = request.PaymentReceipt;
                await _context.BeginTransactionAsync();
                var entity = new Payment
                {
                    Amount = totalAmount,
                    SubscriptionNo = request.SubscriptionNo,
                    TransactionFee = subscription.TransactionFee,
                    CreatedBy = subscriber.Email,
                    Description = "Subscription payment using "+ paymentChannel.Name,
                    PaymentStatus = PaymentStatus.Success,
                    PaymentStatusDesc = PaymentStatus.Success.ToString(),
                    LogDate = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    CurrencyCode = request.CurrencyCode.ToString(),
                    SubscriberId = request.SubscriberId,
                    SubscriptionId = subscription.Id,
                    UserId = request.UserId
                };

                _context.Payments.Add(entity);
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
                    Body2 = "",
                    Body3 = "",
                    ButtonText = "Subscription Payment",
                    ImageSource = _configuration["SVG:PaymentReceived"],
                    DisplayButton = "display:none;"
                };
                var img = _configuration["SVG:EmailVerification"];
                await _emailService.SendEmail(email);
                return Result.Success("Payment Evidence uploaded successfully");

            }
            catch (Exception ex)
            {
                return Result.Failure(ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
