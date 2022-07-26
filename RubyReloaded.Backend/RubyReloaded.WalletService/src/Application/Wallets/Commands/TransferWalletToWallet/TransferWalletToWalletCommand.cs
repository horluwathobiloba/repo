using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Wallets.Commands.CommandTransfer;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Commands.TransferWalletToWallet
{
    public class TransferWalletToWalletCommand:IRequest<Result>
    {
        public int SenderWalletId { get; set; }
        public int RecieverWalletId { get; set; }
        public string Amount { get; set; }
        public string UserId { get; set; }
        public string CurrencyCode { get; set; }
        public string Narration { get; set; }
        public string DeviceId { get; set; }
        public TransactionType TransactionType { get; set; }
    }

    public class TransferWalletToWalletCommandHandler : IRequestHandler<TransferWalletToWalletCommand, Result>
    {
        private readonly IAPIClientService _apiClient;
        private readonly IApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        public TransferWalletToWalletCommandHandler(IAPIClientService aPIClientService, IApplicationDbContext context, 
            IUtilityService utilityService, IConfiguration configuration,IAuthService authService, INotificationService notificationService)
        {
            _apiClient = aPIClientService;
            _context = context;
            _utilityService = utilityService;
            _configuration = configuration;
            _authService=authService;
            _notificationService = notificationService;
        }
        public async Task<Result> Handle(TransferWalletToWalletCommand request, CancellationToken cancellationToken)
        {
            
            var senderWallet = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == request.SenderWalletId);
            var recieverWallet = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == request.RecieverWalletId);
            //This no longer applies
            var transferRequest = new TransferCommand
            {
                UserId = request.UserId,
                beneficiaryBank = _configuration["Providus:bankcode"],
                beneficiaryAccountName = recieverWallet.AccountNumber,
                currencyCode = request.CurrencyCode,
                narration = request.Narration,
                beneficiaryAccountNumber = recieverWallet.AccountNumber,
                transactionAmount = request.Amount,
                TransactionType=request.TransactionType,
                ReciepientName= recieverWallet.UserName,
            };
            var transferHandler =await new TransferCommandHandler(_apiClient, _context, _utilityService, _configuration,_notificationService,_authService).Handle(transferRequest,cancellationToken);
            return transferHandler;
        }
    }
}
 