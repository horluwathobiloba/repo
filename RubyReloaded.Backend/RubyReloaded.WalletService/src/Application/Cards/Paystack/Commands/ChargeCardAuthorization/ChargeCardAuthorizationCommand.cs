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

namespace RubyReloaded.WalletService.Application.Cards.Commands
{
    public class ChargeCardAuthorizationCommand : AuthToken, IRequest<Result>
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string AuthorizationCode { get; set; }
        public int WalletId { get; set; }
        public int PaymentChannelId { get; set; }
    }

    public class ChargeAuthorizationPaystackCommandHandler : IRequestHandler<ChargeCardAuthorizationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IPaystackService _paystackService;
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;

        public ChargeAuthorizationPaystackCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                         IPaystackService paystackService, IStringHashingService stringHashingService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _paystackService = paystackService;
            _stringHashingService = stringHashingService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(ChargeCardAuthorizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO : Get User ID
                var wallet = await _context.Wallets.Where(a => a.Id == request.WalletId && a.UserId == request.UserId && a.Status == Status.Active).FirstOrDefaultAsync();
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Details");
                };
                var cardAuthorization = await _context.CardAuthorizations.Where(a => a.AuthorizationCode == request.AuthorizationCode &&
                                       a.UserId == request.UserId && a.Status == Status.Active).FirstOrDefaultAsync();
                if (cardAuthorization == null)
                {
                    return Result.Failure("Invalid Card Authorization Code");
                }
                if (request.CurrencyCode != CurrencyCode.NGN)
                {
                    return Result.Failure("Invalid Currency Code for Paystack payment");
                }
                
                var hash = (request.Email + request.UserId + DateTime.Now.Ticks).ToString();
                hash = _stringHashingService.CreateDESStringHash(hash);
                hash = HttpUtility.UrlEncode(hash);
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    AuthorizationCode = request.AuthorizationCode,
                    Amount = request.Amount * 100,//kobo equivalent
                    CurrencyCode = request.CurrencyCode.ToString(),
                    Email = wallet.Email,
                    ClientReferenceId = string.Concat("REVENT_CARDCHARGE", Guid.NewGuid()),
                    CallBackUrl = _configuration["Paystack:CallBackUrl"] + "/?item=" + hash
                };
                var response = await _paystackService.ChargeAuthorization(paymentIntentVm);
                if (!response.status)
                {
                    return Result.Failure("Charge card authorization on Paystack with reference number " + paymentIntentVm.ClientReferenceId + "is " + response.data.status + " with message " + response.message);
                }

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
                    PaymentChannelId= request.PaymentChannelId
                };

                _context.WalletTransactions.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Charge Card Authorization on Paystack failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
