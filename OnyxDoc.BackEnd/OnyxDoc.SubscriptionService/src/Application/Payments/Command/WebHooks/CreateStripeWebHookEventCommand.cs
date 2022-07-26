using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using Stripe;
using OnyxDoc.SubscriptionService.Application.Payments.Commands.WebHooks;
using OnyxDoc.SubscriptionService.Application.Payments.Commands.UpdatePayment;
using OnyxDoc.SubscriptionService.Application.PaymentResponses.Commands;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands.WebHooks
{
    public class CreateStripeWebHookEventCommand : IRequest<(UpdatePaymentWithStripeResponseCommand, PaymentIntent, CreateStripePaymentResponseCommand)>
    {
        public Event StripeEvent { get; set; }
    }

}
public class CreateStripeWebHookEventCommandHandler : IRequestHandler<CreateStripeWebHookEventCommand, (UpdatePaymentWithStripeResponseCommand, PaymentIntent, CreateStripePaymentResponseCommand)>
{


    public CreateStripeWebHookEventCommandHandler()
    {

    }

    public async Task<(UpdatePaymentWithStripeResponseCommand, PaymentIntent, CreateStripePaymentResponseCommand)> Handle(CreateStripeWebHookEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = request.StripeEvent.Data.Object as PaymentIntent;
            // Start updating the payment object first
            var paymentCommand = new UpdatePaymentWithStripeResponseCommand();
            paymentCommand.StripeStatus = payment.Status;
            paymentCommand.PaymentIntentId = payment.Id;
            paymentCommand.PaymentStatus = GetPaymentStatus(payment.Status);
            paymentCommand.PaymentStatusDesc = paymentCommand.PaymentStatus.ToString();

            switch (request.StripeEvent.Type)
            {
                case Events.PaymentIntentCanceled:
                    paymentCommand.CancelledDate = DateTime.Now;

                    break;
                case Events.ChargeSucceeded:
                    paymentCommand.CompletedDate = DateTime.Now;

                    break;
                case Events.PaymentIntentCreated:
                    paymentCommand.StripeCreatedDate = payment.Created;
                    paymentCommand.PaymentDate = payment.Created;

                    break;
                case Events.PaymentIntentRequiresAction:
                case Events.PaymentIntentProcessing:
                    //do nothing

                    break;
                case Events.PaymentIntentSucceeded:
                    paymentCommand.CompletedDate = DateTime.Now;

                    break;

                case Events.PaymentIntentPaymentFailed:
                    paymentCommand.FailedDate = DateTime.Now;

                    break;
                default:
                    // Unexpected event type 
                    // do nothing
                    Console.WriteLine("Unhandled event type: {0}", request.StripeEvent.Type);
                    break;
            }

            if (paymentCommand.PaymentStatus == PaymentStatus.Reversed)
            {
                paymentCommand.ReversedDate = DateTime.Now;
            }

            //
            //start updating the payment response
            var command = new CreateStripePaymentResponseCommand
            {
                Amount = payment.Amount,
                AmountCaptured = payment.AmountCapturable,
                ApiVersion = request.StripeEvent.ApiVersion,
                Application = payment.Application.Name,
                ApplicationFee = payment.ApplicationFeeAmount,
                ApplicationFeeAmount = payment.ApplicationFeeAmount,
                BillingDetailsAddress = null,
                BillingDetailsCity = null,
                BillingDetailsCountry = null,
                BillingDetailsEmail = null,
                BillingDetailsLine1 = null,
                BillingDetailsLine2 = null,
                BillingDetailsName = null,
                BillingDetailsPhone = null,
                BillingDetailsPostalCode = null,
                BillingDetailsState = null,
                CalculatedStatementDescriptor = payment.StatementDescriptor,
                Captured = request.StripeEvent.RawJObject.HasValues ? request.StripeEvent.RawJObject.Value<bool>("captured") : false,
                Currency = payment.Currency,
                Customer = payment.Customer?.Name,
                Description = payment.Description,
                EventType = request.StripeEvent.Object,
                IdempotencyKey = request.StripeEvent.Request.IdempotencyKey,
                LiveMode = request.StripeEvent.Livemode,
                OnBehalfOf = null,
                Order = null,
                PaymentCreatedDate = payment.Created,
                PaymentIntent_LiveMode = payment.Livemode,
                PaymentMethod = payment.PaymentMethodId,
                PaymentStatus = GetPaymentStatus(payment.Status),
                PendingWebHooks = request.StripeEvent.PendingWebhooks,
                ReceiptEmail = payment.ReceiptEmail,
                Review = null,
                RequestId = request.StripeEvent.Request.Id,
                SessionId = payment.Id,
                Source = null,
                SourceTransfer = null,
                StatementDescriptor = payment.StatementDescriptor,
                StatementDescriptorSuffix = payment.StatementDescriptorSuffix,
                StripeStatus = payment.Status,
                TransferData = null,
                TransferGroup = payment.TransferGroup,
                //Url = p.Url
            };

            if (payment.RawJObject.HasValues)
            {
                var obj = payment.RawJObject;
                command.AmountRefunded = obj.Value<long>("amount_refunded");
                command.BalanceTransaction = obj.Value<string>("balance_transaction");
                command.Destination = obj.Value<string>("destination");
                command.Dispute = obj.Value<string>("dispute");
                command.Disputed = obj.Value<bool>("disputed");
                command.FailurCode = obj.Value<string>("failure_code");
                command.FailureDetails = obj.Value<string>("failure_details");
                command.FailurMessage = obj.Value<string>("failure_message");
                command.Paid = obj.Value<bool>("paid");
                command.PaymentIntent = obj.Value<string>("payment_intent");
                command.PaymentIntentId = obj.Value<string>("payment_intent");
                command.ReceiptNumber = obj.Value<string>("receipt_number");
                command.ReceiptUrl = obj.Value<string>("receipt_url");
                command.Refunded = obj.Value<bool>("refunded");

                var outcome = obj.Value<Stripe.ChargeOutcome>("outcome");

                if (outcome != null)
                {
                    command.NetworkStatus = outcome.NetworkStatus;
                    command.Reason = outcome.Reason;
                    command.RiskLevel = outcome.RiskLevel;
                    command.RiskScore = outcome.RiskScore;
                    command.SellerMessage = outcome.SellerMessage;
                    command.OutcomeType = outcome.Type;
                }

                var shipping = obj.Value<Stripe.Shipping>("shipping");

                if (shipping != null)
                {
                    command.ShippingAddressLine1 = shipping.Address.Line1;
                    command.ShippingAddressLine2 = shipping.Address.Line2;
                    command.ShippingAddressCity = shipping.Address.City;
                    command.ShippingAddressPostalCode = shipping.Address.PostalCode;
                    command.ShippingAddressState = shipping.Address.State;
                    command.ShippingAddressCountry = shipping.Address.Country;
                }

            }
            command.StripeStatus = payment.Status;
            command.PaymentStatus = GetPaymentStatus(payment.Status);
            command.PaymentIntentId = payment.Id;

            // payment_method_details
            if (payment.PaymentMethod != null)
            {
                command.PaymentMethodType = payment.PaymentMethod?.Type ?? null;
                if (payment.PaymentMethod.Card != null)
                {
                    command.PM_Card_Address_Line1_Check = payment.PaymentMethod.Card.Checks.AddressLine1Check;
                    command.PM_Card_Address_PostalCode_Check = payment.PaymentMethod.Card.Checks.AddressPostalCodeCheck;
                    command.PM_Card_Brand = payment.PaymentMethod.Card.Brand;
                    command.PM_Card_Country = payment.PaymentMethod.Card.Country;
                    command.PM_Card_Cvc_Check = payment.PaymentMethod.Card.Checks.CvcCheck;
                    command.PM_Card_ExpMonth = payment.PaymentMethod.Card.ExpMonth;
                    command.PM_Card_ExpYear = payment.PaymentMethod.Card.ExpYear;
                    command.PM_Card_Fingerprint = payment.PaymentMethod.Card.Fingerprint;
                    command.PM_Card_Funding = payment.PaymentMethod.Card.Funding;
                    command.PM_Card_Last4 = payment.PaymentMethod.Card.Last4;
                    command.PM_Card_Network = payment.PaymentMethod.Card.Issuer;
                    command.PM_Card_Three_D_Secure = payment.PaymentMethod.Card.ThreeDSecureUsage.Supported;
                    command.PM_Card_Wallet = payment.PaymentMethod.Card.Wallet.Type;
                }
            }

            return (paymentCommand, payment, command);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private PaymentStatus GetPaymentStatus(string StripeStatus)
    {
        switch (StripeStatus)
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


