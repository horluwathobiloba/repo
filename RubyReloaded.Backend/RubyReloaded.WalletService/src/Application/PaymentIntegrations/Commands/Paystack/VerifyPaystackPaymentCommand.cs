using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RubyReloaded.WalletService.Application.PaymentIntegrations.Commands
{
    public class VerifyPaystackPaymentCommand : AuthToken, IRequest<Result>
    {
        public string  Reference { get; set; }
    }

    public class VerifyPaystackPaymentCommandHandler : IRequestHandler<VerifyPaystackPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IPaystackService _paystackService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public VerifyPaystackPaymentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                      IPaystackService paystackService, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _paystackService = paystackService;
            _emailService = emailService;
            _configuration = configuration;
    }

        public async Task<Result> Handle(VerifyPaystackPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
               //TODO: Wallet Transaction
                //check if any Payment already exists. If yes, then return a failure response else go ahead and create the Payment
                var existingWalletTransaction = await _context.WalletTransactions.Include(a=>a.Wallet).FirstOrDefaultAsync(a => a.ExternalTransactionReference == request.Reference && a.Status == Status.Active);

                if (existingWalletTransaction != null && existingWalletTransaction.TransactionStatus == TransactionStatus.Success)
                {
                    return Result.Failure($"Wallet Funding for : {existingWalletTransaction.Wallet.WalletAccountNumber} with transaction reference : " +
                        $"{existingWalletTransaction.TransactionReference} has already been processed successfully. Please create a new subscription before proceeding to pay or contact support for more assistance.");
                }

                await _context.BeginTransactionAsync();
                var cardAuthorization = await _context.CardAuthorizations.Where(a => a.ReferenceId == request.Reference).FirstOrDefaultAsync();
                var paymentAmount = 0m;
                var code = CurrencyCode.NGN.ToString();
                var email = "";
                var referenceId = "";
                var hash = "";
                var walletEmail = "";
                var walletName = "";
                if (existingWalletTransaction == null)
                {
                    if (cardAuthorization == null)
                    {
                        return Result.Failure("Invalid wallet transaction specified for verification");
                    }
                    paymentAmount = cardAuthorization.Amount;
                    email = cardAuthorization.CreatedByEmail;
                    referenceId = cardAuthorization.ReferenceId;
                    hash = cardAuthorization.Hash;
                    code = cardAuthorization.CurrencyCode;
                }
                else
                {

                    var wallet = await _context.Wallets.Where(a => a.Id == existingWalletTransaction.Wallet.Id).FirstOrDefaultAsync();
                    if (wallet == null)
                    {
                        return Result.Failure("Invalid Wallet Details");
                    };
                    paymentAmount = existingWalletTransaction.TransactionAmount;
                    email = existingWalletTransaction.CreatedByEmail;
                    referenceId = existingWalletTransaction.ExternalTransactionReference;
                    hash = existingWalletTransaction.Hash;
                    code = existingWalletTransaction.CurrencyCodeDesc;
                }

                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    Amount = paymentAmount,//kobo equivalent
                    CurrencyCode = code,
                    Email = email,
                    ClientReferenceId = referenceId,
                    CallBackUrl = _configuration["Paystack:CallBackUrl"] +"/?item=" + hash
                };
                var verifyResponse =  await _paystackService.GetTransactionStatus(paymentIntentVm);
                if (!verifyResponse.status)

                {
                    return Result.Failure("Verifying Paystack Payment with reference number " + existingWalletTransaction.TransactionReference + "is " + verifyResponse.status.ToString() + " with message " + verifyResponse.message);
                }
                if (existingWalletTransaction != null)
                {
                    if (Convert.ToDecimal(verifyResponse.data.requested_amount) != existingWalletTransaction.TransactionAmount)
                    {
                        return Result.Failure("Error with transaction reference " + existingWalletTransaction.TransactionReference + "Amount from Paystack is not equal to subscription amount. Please contact support");
                    }
                }
                if (cardAuthorization != null)
                {
                    if (Convert.ToDecimal(verifyResponse.data.requested_amount) != cardAuthorization.Amount)
                    {
                        return Result.Failure("Error with transaction reference " + cardAuthorization.ReferenceId + "Amount from Paystack is not equal to card authorization charge amount. Please contact support");
                    }
                    if (verifyResponse.data.authorization == null)
                    {
                        return Result.Failure($"Authorization Failed from Paystack");
                    }
                    if (cardAuthorization != null)
                    {
                        cardAuthorization.CountryCode = verifyResponse.data.authorization.country_code;
                        cardAuthorization.LastDigits = verifyResponse.data.authorization.last4;
                        cardAuthorization.AccountName = verifyResponse.data.authorization.account_name;
                        cardAuthorization.AuthorizationCode = verifyResponse.data.authorization.authorization_code;
                        cardAuthorization.Bank = verifyResponse.data.authorization.bank;
                        cardAuthorization.Bin = verifyResponse.data.authorization.bin;
                        cardAuthorization.Brand = verifyResponse.data.authorization.brand;
                        cardAuthorization.CardType = verifyResponse.data.authorization.card_type;
                        cardAuthorization.Channel = verifyResponse.data.authorization.channel;
                        cardAuthorization.ExpiryMonth = verifyResponse.data.authorization.exp_month;
                        cardAuthorization.ExpiryYear = verifyResponse.data.authorization.exp_year;
                        cardAuthorization.Signature = verifyResponse.data.authorization.signature;
                        cardAuthorization.ReferenceId = paymentIntentVm.ClientReferenceId;
                        cardAuthorization.Amount = paymentIntentVm.Amount;
                        cardAuthorization.LastModifiedBy = cardAuthorization.UserId;
                        cardAuthorization.LastModifiedDate = DateTime.Now;
                        cardAuthorization.CreatedDate = DateTime.Now;
                        cardAuthorization.CurrencyCode = cardAuthorization.CurrencyCode;
                        cardAuthorization.Status = Status.Active;
                        cardAuthorization.StatusDesc = Status.Active.ToString();
                        _context.CardAuthorizations.Update(cardAuthorization);
                        await _context.SaveChangesAsync(cancellationToken);
                        await _context.CommitTransactionAsync();
                        return Result.Success($"Card Authorization was successfully added",cardAuthorization);
                    }
                }

               
                var message = "";
                switch (verifyResponse.message)
                {
                    case "success":
                        existingWalletTransaction.TransactionStatus = TransactionStatus.Success;
                        message = "Your subscription payment has been fulfilled successfully";
                        break;
                    case "abandoned":
                        existingWalletTransaction.TransactionStatus = TransactionStatus.Cancelled;
                        message = "Oops! You abandoned your subscription payment";
                        break;
                    case "cancelled":
                        existingWalletTransaction.TransactionStatus = TransactionStatus.Cancelled;
                        message = "Oops! You cancelled your subscription payment";
                        break;
                    case "failed":
                        existingWalletTransaction.TransactionStatus = TransactionStatus.Failed;
                        message = "Oops! You subscription payment failed";
                        break;
                    case "failure":
                        existingWalletTransaction.TransactionStatus = TransactionStatus.Failed;
                        message = "Oops! You subscription payment failed";
                        break;
                    case "processing":
                        existingWalletTransaction.TransactionStatus = TransactionStatus.Processing;
                        message = "Your subscription payment is processing";
                        break;
                    default:
                        break;
                }

                existingWalletTransaction.TransactionStatusDesc = existingWalletTransaction.TransactionStatus.ToString();
                _context.WalletTransactions.Update(existingWalletTransaction);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var verifyTransactionEmail = new EmailVm
                {
                    Application = _configuration["Email:AppName"],
                    Subject = "Wallet Funding",
                    RecipientEmail = walletEmail,
                    RecipientName = walletName,
                    Text = existingWalletTransaction.TransactionStatus == TransactionStatus.Success ? "Hurray!" : "",
                    Body1 = message,
                    Body2 = "",
                    Body3 = "",
                    ButtonText = "Wallet Funding",
                    ImageSource = _configuration["SVG:PaymentReceived"],
                    DisplayButton = "display:none;"
                };
                var img = _configuration["SVG:EmailVerification"];
                var emailResp = await _emailService.SendEmail(verifyTransactionEmail);

                return Result.Success(verifyResponse);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Verify Paystack Payment failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
