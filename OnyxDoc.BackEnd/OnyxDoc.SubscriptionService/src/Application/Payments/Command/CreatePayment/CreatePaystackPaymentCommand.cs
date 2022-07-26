using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Domain.Common;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using Newtonsoft.Json;
using System.Web;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class CreatePaystackPaymentCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int PaymentChannelId { get; set; }
        public string SubscriptionNo { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public long Quantity => 1;

        [JsonIgnore]
        public string Mode => "Payment";

        [JsonIgnore]
        public string Description => $"Payment for Subscription No: {this.SubscriptionNo}";
    }

    public class CreatePaystackPaymentCommandHandler : IRequestHandler<CreatePaystackPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPaystackService _iPaystackService;
        private readonly IAuthService _authService;
        private readonly IStringHashingService _stringHashingService;
        public CreatePaystackPaymentCommandHandler(IApplicationDbContext context, IConfiguration configuration, IPaystackService iPaystackService,
                                                  IAuthService authService, IStringHashingService stringHashingService)
        {
            _context = context;
            _configuration = configuration;
            _iPaystackService = iPaystackService;
            _authService = authService;
            _stringHashingService = stringHashingService;
        }

        public async Task<Result> Handle(CreatePaystackPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CurrencyCode != CurrencyCode.NGN)
                {
                    return Result.Failure("Invalid Currency Code for paystack payment");
                }

                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionNo == request.SubscriptionNo);

                if (subscription == null)
                {
                    return Result.Failure("Invalid subscriber or subscription number specified.");
                }

                SubscriberDto subscriber = (await _authService.GetSubscriberAsync(request.AccessToken, request.SubscriberId, request.UserId)).Entity;
                var paymentChannel = await _context.PaymentChannels.FirstOrDefaultAsync(a => a.CurrencyCode == request.CurrencyCode && a.Id == request.PaymentChannelId);

                var subscriptionAmount = subscription.TotalAmount + paymentChannel.TransactionFee;

                //check if any Payment already exists.. If yes, then return a failure response else go ahead and create the Payment
                var exists = await _context.Payments.AnyAsync(a => a.SubscriberId == subscriber.Id && a.SubscriptionNo.ToUpper() == request.SubscriptionNo.ToUpper()
                && (a.PaymentStatus == PaymentStatus.Processing || a.PaymentStatus == PaymentStatus.Success) && a.Status == Status.Active);

                if (exists)
                {
                    return Result.Failure(new string[] { $"Create new Payment failed because a payment with the same subscription no: {request.SubscriptionNo} already exists. Please create a new subscription before proceeding to pay or contact support for more assistance." });
                }
                var hash = (request.SubscriptionNo + request.SubscriberId + request.UserId + DateTime.Now.Ticks).ToString();
                hash = _stringHashingService.CreateDESStringHash(hash);
                hash = HttpUtility.UrlEncode(hash);
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    SubscriptionAmount = subscriptionAmount * 100,//kobo equivalent
                    CurrencyCode = request.CurrencyCode.ToString(),
                    SubscriberId = request.SubscriberId,
                    TransactionFee = subscription.TransactionFee,
                    SubscriptionNo = request.SubscriptionNo,
                    Email = subscriber?.ContactEmail,
                    ClientReferenceNo = string.Format("{0:ddMMyyyyHHmmssfff}-{1}", DateTime.Now, request.SubscriptionNo),
                    CallBackUrl = _configuration["Paystack:CallBackUrl"] + "/paystack?item=" + hash
                };
                subscription.TotalAmount = subscriptionAmount;
                subscription.TransactionFee = paymentChannel.TransactionFee;
                _context.Subscriptions.Update(subscription);
                var result = await _iPaystackService.InitializePayment(paymentIntentVm);

                if (result.Status == false)
                {
                    return Result.Failure("Initialize paystack transaction with reference nummber " + paymentIntentVm.ClientReferenceNo + "is " + result.Data?.Status + " with message " + result.Message);
                }

                var entity = new Payment
                {                    
                    Amount = subscriptionAmount,
                    Description = paymentIntentVm.Description,
                    SubscriberId = request.SubscriberId,
                    SubscriptionId = subscription.Id,
                    AuthorizationUrl = result.Data?.Authorization_Url,
                    SessionId = result.Data?.Access_Code,
                    ReferenceNo =  result.Data?.Reference,                   
                    CurrencyCode = request.CurrencyCode.ToString(),
                    Quantity = request.Quantity,
                    Mode = request.Mode,
                    SubscriptionNo = request.SubscriptionNo,
                    ClientReferenceNo = paymentIntentVm.ClientReferenceNo,
                    TransactionFee = subscription.TransactionFee,
                    CreatedBy = subscriber.Email,
                    PaymentStatus = PaymentStatus.Initiated,
                    PaymentStatusDesc = PaymentStatus.Initiated.ToString(),
                    LogDate = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    UserId = request.UserId
                };

                _context.Payments.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Payment initialized successfully!", result);

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment initialization failed!", ex?.Message ?? ex?.InnerException.Message
    });
            }
        }
    }
}