using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using OnyxDoc.SubscriptionService.Application.Common.Models; 
using AutoMapper;

namespace OnyxDoc.SubscriptionService.Application.PaymentResponses.Commands
{
    public class CreateStripePaymentResponseCommand : IRequest<Result>
    {
        #region Class Properties
        public int PaymentId { get; set; }
        public string SessionId { get; set; }
        public string ApiVersion { get; set; }
        public DateTime SessionCreatedDate { get; set; }
        public string PaymentIntentId { get; set; }
        public long Amount { get; set; }
        public long AmountCaptured { get; set; }
        public long AmountRefunded { get; set; }
        public string Application { get; set; }
        public long? ApplicationFee { get; set; }
        public long? ApplicationFeeAmount { get; set; }

        #region Billing Details
        public string BalanceTransaction { get; set; }
        public string BillingDetailsAddress { get; set; }
        public string BillingDetailsCity { get; set; }
        public string BillingDetailsCountry { get; set; }
        public string BillingDetailsLine1 { get; set; }
        public string BillingDetailsLine2 { get; set; }
        public string BillingDetailsPostalCode { get; set; }
        public string BillingDetailsState { get; set; }

        public string BillingDetailsEmail { get; set; }
        public string BillingDetailsName { get; set; }
        public string BillingDetailsPhone { get; set; }
        #endregion

        public string CalculatedStatementDescriptor { get; set; }
        public bool Captured { get; set; }
        public DateTime PaymentCreatedDate { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public string Dispute { get; set; }
        public bool Disputed { get; set; }
        public string FailurCode { get; set; }
        public string FailurMessage { get; set; }
        public string FailureDetails { get; set; }
        public string FailureCode { get; set; }
        public string InvoicePdf { get; set; }
        public bool PaymentIntent_LiveMode { get; set; }
        public string OnBehalfOf { get; set; }
        public string Order { get; set; }

        #region  Outcome
        public string NetworkStatus { get; set; }
        public string Reason { get; set; }
        public string RiskLevel { get; set; }
        public long RiskScore { get; set; }
        public string SellerMessage { get; set; }
        public string OutcomeType { get; set; }
        #endregion

        public bool Paid { get; set; }
        public string PaymentIntent { get; set; }
        public string PaymentMethod { get; set; }
        public string PM_Card_Brand { get; set; }
        public string PM_Card_Address_Line1_Check { get; set; }
        public string PM_Card_Address_PostalCode_Check { get; set; }
        public string PM_Card_Cvc_Check { get; set; }
        public string PM_Card_Country { get; set; }
        public long PM_Card_ExpMonth { get; set; }
        public long PM_Card_ExpYear { get; set; }
        public string PM_Card_Fingerprint { get; set; }
        public string PM_Card_Funding { get; set; }
        public string PM_Card_Last4 { get; set; }
        public string PM_Card_Network { get; set; }
        public bool PM_Card_Three_D_Secure { get; set; }
        public string PM_Card_Wallet { get; set; }
        public string PaymentMethodType { get; set; }
        public string ReceiptEmail { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptUrl { get; set; }
        public bool Refunded { get; set; }
        public bool HasMore { get; set; }
        public string RefundUrl { get; set; }
        public long TotalCount { get; set; }
        public string Review { get; set; }
        public string ShippingAddressCity { get; set; }
        public string ShippingAddressCountry { get; set; }
        public string ShippingAddressLine1 { get; set; }
        public string ShippingAddressLine2 { get; set; }
        public string ShippingAddressPostalCode { get; set; }
        public string ShippingAddressState { get; set; }
        public string ShippingCarrier { get; set; }
        public string ShippingName { get; set; }
        public string ShippingPhone { get; set; }
        public string ShippingTrackingNumber { get; set; }
        public string Source { get; set; }
        public string SourceTransfer { get; set; }
        public string StatementDescriptor { get; set; }
        public string StatementDescriptorSuffix { get; set; }
        public string StripeStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }
        public string TransferData { get; set; }
        public string TransferGroup { get; set; }
        public bool LiveMode { get; set; }
        public long PendingWebHooks { get; set; }
        public string RequestId { get; set; }
        public string IdempotencyKey { get; set; }         
        public string EventType { get; set; }
        
        #endregion
    }

    public class CreateStripePaymentResponseCommandHandler : IRequestHandler<CreateStripePaymentResponseCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateStripePaymentResponseCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateStripePaymentResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any Payment already exists.. If yes, then return a failure response else go ahead and create the Payment
                var entity = new StripePaymentResponse();

                try
                { 
                    #region Assign Values Explicitly
                    entity.Amount = request.Amount;
                    entity.AmountCaptured = request.AmountCaptured;
                    entity.AmountRefunded = request.AmountRefunded;
                    entity.ApiVersion = request.ApiVersion;
                    entity.Application = request.Application;
                    entity.ApplicationFee = request.ApplicationFee;
                    entity.ApplicationFeeAmount = request.ApplicationFeeAmount;
                    entity.BalanceTransaction = request.BalanceTransaction;
                    entity.BillingDetailsAddress = request.BillingDetailsAddress;
                    entity.BillingDetailsCity = request.BillingDetailsCity;
                    entity.BillingDetailsCountry = request.BillingDetailsCountry;
                    entity.BillingDetailsEmail = request.BillingDetailsEmail;
                    entity.BillingDetailsLine1 = request.BillingDetailsLine1;
                    entity.BillingDetailsLine2 = request.BillingDetailsLine2;
                    entity.BillingDetailsName = request.BillingDetailsName;
                    entity.BillingDetailsPhone = request.BillingDetailsPhone;
                    entity.BillingDetailsPostalCode = request.BillingDetailsPostalCode;
                    entity.BillingDetailsState = request.BillingDetailsState;
                    entity.CalculatedStatementDescriptor = request.CalculatedStatementDescriptor;
                    entity.Captured = request.Captured;
                    entity.Currency = request.Currency;
                    entity.Customer = request.Customer;
                    entity.Description = request.Description;
                    entity.Destination = request.Destination;
                    entity.Dispute = request.Dispute;
                    entity.Disputed = request.Disputed;
                    entity.EventType = request.EventType;
                    entity.RequestId = request.RequestId;
                    entity.Reason = request.Reason;
                    entity.FailurCode = request.FailurCode;
                    entity.FailureCode = request.FailureCode;
                    entity.FailurMessage = request.FailurMessage;
                    entity.HasMore = request.HasMore;
                    entity.IdempotencyKey = request.IdempotencyKey;
                    entity.InvoicePdf = request.InvoicePdf;
                    entity.LiveMode = request.LiveMode;
                    entity.Name = request.Description;
                    entity.NetworkStatus = request.NetworkStatus;
                    entity.OnBehalfOf = request.OnBehalfOf;
                    entity.Order = request.Order;
                    entity.OutcomeType = request.OutcomeType;
                    entity.Paid = request.Paid;
                    entity.PaymentCreatedDate = request.PaymentCreatedDate;
                    entity.PaymentId = request.PaymentId;
                    entity.PaymentIntent = request.PaymentIntent;
                    entity.PaymentIntentId = request.PaymentIntentId;
                    entity.PaymentIntent_LiveMode = request.PaymentIntent_LiveMode;
                    entity.PaymentMethod = request.PaymentMethod;
                    entity.PaymentMethodType = request.PaymentMethodType;
                    entity.PaymentStatus = request.PaymentStatus;
                    entity.PendingWebHooks = request.PendingWebHooks;
                    entity.PM_Card_Address_Line1_Check = request.PM_Card_Address_Line1_Check;
                    entity.PM_Card_Address_PostalCode_Check = request.PM_Card_Address_PostalCode_Check;
                    entity.PM_Card_Brand = request.PM_Card_Brand;
                    entity.PM_Card_Country = request.PM_Card_Country;
                    entity.PM_Card_Cvc_Check = request.PM_Card_Cvc_Check;
                    entity.PM_Card_ExpMonth = request.PM_Card_ExpMonth;
                    entity.PM_Card_ExpYear = request.PM_Card_ExpYear;
                    entity.PM_Card_Fingerprint = request.PM_Card_Fingerprint;
                    entity.PM_Card_Funding = request.PM_Card_Funding;
                    entity.PM_Card_Last4 = request.PM_Card_Last4;
                    entity.PM_Card_Network = request.PM_Card_Network;
                    entity.PM_Card_Three_D_Secure = request.PM_Card_Three_D_Secure;
                    entity.PM_Card_Wallet = request.PM_Card_Wallet;
                    entity.ReceiptEmail = request.ReceiptEmail;
                    entity.ReceiptNumber = request.ReceiptNumber;
                    entity.ReceiptUrl = request.ReceiptUrl;
                    entity.Refunded = request.Refunded;
                    entity.Review = request.Review;
                    entity.RiskLevel = request.RiskLevel;
                    entity.RiskScore = request.RiskScore;
                    entity.SellerMessage = request.SellerMessage;
                    entity.SessionCreatedDate = request.SessionCreatedDate;
                    entity.SessionId = request.SessionId;
                    entity.ShippingAddressCity = request.ShippingAddressCity;
                    entity.ShippingAddressCountry = request.ShippingAddressCountry;
                    entity.ShippingAddressLine1 = request.ShippingAddressLine1;
                    entity.ShippingAddressLine2 = request.ShippingAddressLine2;
                    entity.ShippingAddressPostalCode = request.ShippingAddressPostalCode;
                    entity.ShippingAddressState = request.ShippingAddressState;
                    entity.ShippingCarrier = request.ShippingCarrier;
                    entity.ShippingName = request.ShippingName;
                    entity.ShippingPhone = request.ShippingPhone;
                    entity.ShippingTrackingNumber = request.ShippingTrackingNumber;
                    entity.Source = request.Source;
                    entity.SourceTransfer = request.SourceTransfer;
                    entity.StatementDescriptor = request.StatementDescriptor;
                    entity.StatementDescriptorSuffix = request.StatementDescriptorSuffix;
                    entity.StripeStatus = request.StripeStatus;
                    entity.TotalCount = request.TotalCount;
                    entity.TransferData = request.TransferData;
                    entity.TransferGroup = request.TransferGroup;
                    entity.CreatedBy = "SYSTEM";
                    entity.CreatedDate = DateTime.Now;
                    entity.Status = Status.Active;
                    entity.RefundUrl = request.RefundUrl;
                    #endregion
                }
                catch (Exception ex)
                {
                  
                }
                _context.PaymentResponses.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("PaymentResponse created successfully!", entity.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "PaymentResponse creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}