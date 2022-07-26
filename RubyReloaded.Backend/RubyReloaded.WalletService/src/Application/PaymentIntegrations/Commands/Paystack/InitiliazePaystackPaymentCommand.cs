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
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RubyReloaded.WalletService.Application.PaymentIntegrations.Commands
{
    public class InitiliazePaystackPaymentCommand : AuthToken, IRequest<Result>
    {
        public int WalletId { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public TransactionServiceType TransactionServiceType { get; set; }
        public int PaymentChannelId { get; set; }
        [JsonIgnore]
        public bool SaveCardDetails { get; set; }
    }

    public class InitiliazePaystackPaymentCommandHandler : IRequestHandler<InitiliazePaystackPaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IPaystackService _paystackService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;

        public InitiliazePaystackPaymentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                         IPaystackService paystackService, IStringHashingService stringHashingService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _paystackService = paystackService;
            _stringHashingService = stringHashingService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(InitiliazePaystackPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO: get user id
                
                if (request.CurrencyCode != CurrencyCode.NGN)
                {
                    return Result.Failure("Invalid Currency Code for Paystack payment");
                }

                var paymentChannel = await _context.PaymentChannels.Where(a=>a.PaymentGatewayCategory == PaymentGatewayCategory.Paystack).FirstOrDefaultAsync();
                if (paymentChannel != null)
                {
                    request.PaymentChannelId = paymentChannel.Id;
                }
                //check if any wallet service already exists.. If yes, then return a failure response else go ahead and create the Payment
                var wallet = await _context.Wallets.Where(a => a.Id == request.WalletId && a.UserId == request.UserId).FirstOrDefaultAsync();
                if (wallet == null && request.WalletId > 0)
                {
                    return Result.Failure("Invalid Wallet Details");
                };
                //TODO:Check for transaction fee
                var hash = (wallet?.WalletAccountNumber + request.UserId + DateTime.Now.Ticks).ToString();
                hash = _stringHashingService.CreateDESStringHash(hash);
                hash = HttpUtility.UrlEncode(hash);
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    Amount = request.Amount * 100,//kobo equivalent
                    CurrencyCode = request.CurrencyCode.ToString(),
                    Email =  request.Email != null ? request.Email : wallet.Email,
                    ClientReferenceId = string.Concat("REVENT_TRF", Guid.NewGuid()),
                    CallBackUrl = _configuration["Paystack:CallBackUrl"] + "/?item=" + hash
                };
                if (request.SaveCardDetails)
                {
                    paymentIntentVm.ClientReferenceId= string.Concat("REVENT_CARDAUTH", Guid.NewGuid());
                }
                var response = await _paystackService.InitializeTransaction(paymentIntentVm);
                if (!response.status)
                {
                    return Result.Failure("Initialize paystack transaction with reference number " + paymentIntentVm.ClientReferenceId + "is " + response.data.status + " with message " + response.message);
                }
                 
                if (request.SaveCardDetails)
                {
                    CardAuthorization authorization = new CardAuthorization
                    {
                        CountryCode = response.data.authorization?.country_code,
                        LastDigits = response.data.authorization?.last4,
                        AccountName = response.data.authorization?.account_name,
                        AuthorizationCode = response.data.authorization?.authorization_code,
                        Bank = response.data.authorization?.bank,
                        Bin = response.data.authorization?.bin,
                        Brand = response.data.authorization?.brand,
                        CardType = response.data.authorization?.card_type,
                        Channel = response.data.authorization?.channel,
                        ExpiryMonth = response.data.authorization?.exp_month,
                        ExpiryYear = response.data.authorization?.exp_year,
                        Signature = response.data.authorization?.signature,
                        ReferenceId = paymentIntentVm.ClientReferenceId,
                        Amount = paymentIntentVm.Amount,
                        CreatedBy = request.UserId,
                        CreatedByEmail = request.Email,
                        CreatedDate = DateTime.Now,
                        UserId = request.UserId,
                        Status = Status.Inactive,
                        StatusDesc = Status.Inactive.ToString()
                    };
                    await _context.CardAuthorizations.AddAsync(authorization, cancellationToken);
                }
                if ( request.WalletId > 0)
                {
                    var entity = new WalletTransaction
                    {
                        TransactionType = TransactionType.Credit,
                        CreatedByEmail = wallet.Email,
                        ServiceCategory = ServiceCategory.WalletFunding,
                        TransactionAmount = paymentIntentVm.Amount,
                        TransactionTypeDesc = TransactionType.Credit.ToString(),
                        WalletId = wallet.Id,
                        Description = "Wallet Funding using Paystack",
                        CreatedDate = DateTime.Now,
                        TransactionStatus = TransactionStatus.Processing,
                        TransactionStatusDesc = TransactionStatus.Processing.ToString(),
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString(),
                        ExternalTransactionReference = response.data.reference,
                        Hash = hash,
                        PaymentChannelId = paymentChannel.Id,
                        UserId = request.UserId
                    };
                    await _context.WalletTransactions.AddAsync(entity);
                }
                
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Initialize Paystack Payment failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
