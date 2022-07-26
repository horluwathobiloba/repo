using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Customer = OnyxDoc.SubscriptionService.Domain.ViewModels.Customer;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class InitializeFlutterwavePaymentCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public Customer Customer { get; set; }
        public int PaymentChannelId { get; set; }
        public string SubscriptionNo { get; set; }
    }

    public class InitializeFlutterwavePaymentCommandHandler : IRequestHandler<InitializeFlutterwavePaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IFlutterwaveService _flutterwaveService;
        private readonly IStringHashingService _stringHashingService;

        public InitializeFlutterwavePaymentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                          IFlutterwaveService flutterwaveService, IStringHashingService stringHashingService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _flutterwaveService = flutterwaveService;
            _stringHashingService = stringHashingService;
        }

        public async Task<Result> Handle(InitializeFlutterwavePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                if (request.CurrencyCode != CurrencyCode.NGN)
                {
                    return Result.Failure("Invalid Currency Code for Flutterwave payment");
                }

                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.SubscriptionNo == request.SubscriptionNo);

                if (subscription == null)
                {
                    return Result.Failure("Invalid subscriber or subscription number specified.");
                }
                //get subscription plan
                var subscriptionPlan = await _context.SubscriptionPlans.Where(a => a.Id == subscription.SubscriptionPlanId).FirstOrDefaultAsync();
                if (subscriptionPlan == null)
                {
                    return Result.Failure("Invalid subscription plan specified.");
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
                //create flutterwave payment plan
                FlutterwavePaymentPlan paymentPlan = new FlutterwavePaymentPlan
                {
                    Amount = request.Amount,
                    Duration = subscription.PaymentPeriod.ToString(),
                    Interval = subscription.SubscriptionFrequency.ToString(),
                    Name = subscriptionPlan.Name
                };
                var paymentPlanResponse = await _flutterwaveService.CreatePaymentPlan(paymentPlan);
                if (paymentPlanResponse.status != "success")
                {
                    return Result.Failure("Flutterwave payment plan creation is " + paymentPlanResponse.status + " with message " + paymentPlanResponse.message);
                }
                //create the plan on our end
             
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    SubscriptionAmount = totalAmount,//kobo equivalent
                    CurrencyCode = request.CurrencyCode.ToString(),
                    SubscriberId = request.SubscriberId,
                    TransactionFee = subscription.TransactionFee,
                    SubscriptionNo = request.SubscriptionNo,
                    Email = subscriber?.Email,
                    ClientReferenceNo = string.Format("{0:ddMMyyyyHHmmssfff}-{1}", DateTime.Now, request.SubscriptionNo),
                    PaymentPlanId = paymentPlanResponse.data.id != 0 ? 0: paymentPlanResponse.data.id
                };
                subscription.TotalAmount = paymentIntentVm.SubscriptionAmount;
                subscription.TransactionFee = paymentChannel.TransactionFee;
                _context.Subscriptions.Update(subscription);
                var hash = (request.SubscriptionNo + request.SubscriberId + request.UserId + DateTime.Now.Ticks).ToString();
                hash = _stringHashingService.CreateDESStringHash(hash);
                hash = HttpUtility.UrlEncode(hash);
                FlutterwaveRequest transaction = new FlutterwaveRequest
                {
                    Tx_Ref =  Guid.NewGuid().ToString(),
                    Amount = paymentIntentVm.SubscriptionAmount,
                    Currency = request.CurrencyCode.ToString(),
                    Customer = request.Customer,
                     
                };
                if (paymentPlanResponse != null)
                {
                    transaction.Payment_Plan = paymentIntentVm.PaymentPlanId;
                }
               var response = await  _flutterwaveService.InitiatePayment(transaction,hash);
                if (response.status != "success")
                {
                    return Result.Failure("Initialize flutterwave transaction with reference number " + paymentIntentVm.ClientReferenceNo + "is " + response.status + " with message " + response.message);
                }
                if (paymentPlanResponse != null)
                {
                    PGPlan paymentGatewayPlan = new PGPlan
                    {
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = request.Customer.Email,
                        CreatedBy = request.UserId,
                        PaymentGatewayPlanId = paymentPlanResponse.data.id,
                        UserId = request.UserId,
                        PaymentGateway = PaymentGateway.Flutterwave,
                        PaymentGatewayDesc = PaymentGateway.Flutterwave.ToString(),
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString(),
                        SubscriberId = request.SubscriberId,
                        SubscriptionId = subscription.Id,
                    };
                    _context.PGPlans.Add(paymentGatewayPlan);
                }
                    var entity = new Payment
                {
                    Amount = paymentIntentVm.SubscriptionAmount,
                    SubscriptionNo = request.SubscriptionNo,
                    ClientReferenceNo = paymentIntentVm.ClientReferenceNo,
                    AuthorizationUrl = response.data.link,
                    TransactionFee = subscription.TransactionFee,
                    CreatedBy = subscriber.Email,
                    Description = paymentIntentVm.Description,
                    PaymentStatus = PaymentStatus.Processing,
                    PaymentStatusDesc = PaymentStatus.Processing.ToString(),
                    LogDate = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    CurrencyCode = request.CurrencyCode.ToString(),
                    SessionId = response.data.tx_ref,
                    SubscriberId = request.SubscriberId,
                    SubscriptionId = subscription.Id,
                    Hash = hash,
                    UserId = request.UserId
                };

                _context.Payments.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Initialize Flutterwave Payment failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
