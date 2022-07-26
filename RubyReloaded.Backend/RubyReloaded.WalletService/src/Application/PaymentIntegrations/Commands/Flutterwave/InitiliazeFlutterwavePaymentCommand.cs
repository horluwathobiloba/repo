using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class InitiliazeFlutterwavePaymentCommand : AuthToken, IRequest<Result>
    {
        public int WalletId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionServiceType TransactionServiceType { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public int PaymentChannelId { get; set; }
    }

    public class InitiliazeFlutterwavePaymentCommandHandler : IRequestHandler<InitiliazeFlutterwavePaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IFlutterwaveService _flutterwaveService;
        private readonly IStringHashingService _stringHashingService;

        public InitiliazeFlutterwavePaymentCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService,
                                                         IFlutterwaveService flutterwaveService, IStringHashingService stringHashingService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _flutterwaveService = flutterwaveService;
            _stringHashingService = stringHashingService;
        }

        public async Task<Result> Handle(InitiliazeFlutterwavePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //TODO: get user id
                
                if (request.CurrencyCode != CurrencyCode.NGN)
                {
                    return Result.Failure("Invalid Currency Code for Flutterwave payment");
                }
                //check if any wallet service already exists.. If yes, then return a failure response else go ahead and create the Payment
                var wallet = await _context.Wallets.Where(a => a.Id == request.WalletId).FirstOrDefaultAsync();
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Details");
                };
               
                PaymentIntentVm paymentIntentVm = new PaymentIntentVm
                {
                    Amount = request.Amount ,//kobo equivalent
                    CurrencyCode = request.CurrencyCode.ToString(),
                    Email = wallet.Email,
                    ClientReferenceId = string.Concat("REVENT_TRF", Guid.NewGuid())
                };
                //TODO:Check for transaction fee
                FlutterwaveRequest transaction = new FlutterwaveRequest
                {
                    Tx_Ref =  Guid.NewGuid().ToString(),
                    Amount = request.Amount*100,
                    Currency = request.CurrencyCode.ToString(),
                    Customer = new Customer { Email = wallet.Email, PhoneNumber = wallet.PhoneNumber, Name= wallet.Name}
                };
                var hash = (wallet.WalletAccountNumber + request.UserId + DateTime.Now.Ticks).ToString();
                hash = HttpUtility.UrlEncode( _stringHashingService.CreateDESStringHash(hash));

                var response = await  _flutterwaveService.InitiatePayment(transaction,hash);
                if (response.status != "success")
                {
                    return Result.Failure("Initialize flutterwave transaction with reference number " + paymentIntentVm.ClientReferenceId + "is " + response.status + " with message " + response.message);
                }

                var entity = new WalletTransaction
                {
                    TransactionType = TransactionType.Credit,
                    CreatedByEmail = wallet.Email,
                    ServiceCategory = ServiceCategory.WalletFunding,
                    TransactionAmount = transaction.Amount,
                    TransactionTypeDesc = TransactionType.Credit.ToString(),
                    WalletId = wallet.Id,
                    Description = "Wallet Funding using Flutterwave",
                    CreatedDate = DateTime.Now,
                    TransactionStatus = TransactionStatus.Processing,
                    TransactionStatusDesc = TransactionStatus.Processing.ToString(),
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Hash = hash,
                    PaymentChannelId = request.PaymentChannelId,
                    UserId = request.UserId
                };

                _context.WalletTransactions.Add(entity);
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
