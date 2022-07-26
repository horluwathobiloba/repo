using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RubyReloaded.WalletService.Application.PaymentIntegrations.Commands
{
    public class VerifyFlutterwavePaymentCommand : AuthToken, IRequest<Result>
    {
        public string TransactionId { get; set; }
        public string Hash { get; set; }
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
                //var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                //if (_authService.Subscriber == null)
                //{
                //    return Result.Failure(new string[] { "Invalid subscriber details specified." });
                //}
                //var subscriber = _authService.Subscriber;

                //check if any Payment already exists. If yes, then return a failure response else go ahead and create the Payment
                var existingWalletTransaction = await _context.WalletTransactions.Include(a=>a.Wallet).FirstOrDefaultAsync(a => a.Hash == request.Hash && a.Status == Status.Active);

                if (existingWalletTransaction != null && existingWalletTransaction.TransactionStatus == TransactionStatus.Success && existingWalletTransaction.Hash == request.Hash)
                {
                    return Result.Failure($"Wallet Funding for : {existingWalletTransaction.Wallet.WalletAccountNumber} with transaction id : " +
                        $"{request.TransactionId} has already been processed successfully. Please create a new subscription before proceeding to pay or contact support for more assistance.");
                }
               

                var wallet = await _context.Wallets.Where(a => a.Id == existingWalletTransaction.Wallet.Id).FirstOrDefaultAsync();
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Details");
                };
                if (existingWalletTransaction == null)
                {
                    return Result.Failure("Invalid wallet transaction specified for verification");
                }
                var verifyResponse =  await _flutterwaveService.GetPaymentStatus(request.TransactionId);
                if (verifyResponse.status != "success")
                {
                    return Result.Failure("Verifying Flutterwave Payment with reference number " + existingWalletTransaction.TransactionReference + "is " + verifyResponse.status + " with message " + verifyResponse.message);
                }

                if (Convert.ToDecimal(verifyResponse.data.amount) != existingWalletTransaction.TransactionAmount)
                {
                    return Result.Failure("Error with transaction reference " + existingWalletTransaction.TransactionReference + "Amount from Flutterwave is not equal to subscription amount. Please contact support");
                }

                var message = "";
                switch (verifyResponse.status)
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
                await _context.BeginTransactionAsync();

                _context.WalletTransactions.Update(existingWalletTransaction);

                await _context.SaveChangesAsync(cancellationToken);

                await _context.CommitTransactionAsync();

                var email = new EmailVm
                {
                    Application = _configuration["Email:AppName"],
                    Subject = "Wallet Funding",
                    RecipientEmail = wallet.Email,
                    RecipientName = wallet.Name,
                    Text = existingWalletTransaction.TransactionStatus == TransactionStatus.Success ? "Hurray!" : "",
                    Body1 = message,
                    Body2 = "",
                    Body3 = "",
                    ButtonText = "Wallet Funding",
                    ImageSource = _configuration["SVG:PaymentReceived"],
                    DisplayButton = "display:none;"
                };
                var img = _configuration["SVG:EmailVerification"];
                var emailResp = await _emailService.SendEmail(email);

                return Result.Success(verifyResponse);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Verify Flutterwave Payment failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
