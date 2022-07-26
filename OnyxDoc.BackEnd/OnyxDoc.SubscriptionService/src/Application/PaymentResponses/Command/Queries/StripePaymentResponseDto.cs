using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Application.PaymentResponses.Commands.CreatePaymentResponse;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace OnyxDoc.SubscriptionService.Application.PaymentResponses.Queries.GetPaymentResponses
{
    public class StripePaymentResponseDto : IMapFrom<StripePaymentResponse>
    {
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
        public string PM_Card_Three_D_Secure { get; set; }
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StripePaymentResponse, StripePaymentResponseDto>();
            profile.CreateMap<StripePaymentResponseDto, StripePaymentResponse>();
        }
    }

}
