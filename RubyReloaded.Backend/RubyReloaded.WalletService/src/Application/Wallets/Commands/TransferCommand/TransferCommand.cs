using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.CreateVirtualAccountConfig;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;

using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Commands.CommandTransfer
{
    public class TransferCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string beneficiaryAccountName { get; set; }
        public string transactionAmount { get; set; }
        public string currencyCode { get; set; }
        public string narration { get; set; }
        public string beneficiaryAccountNumber { get; set; }
        public string beneficiaryBank { get; set; }
        public TransactionType TransactionType { get; set; }
        public string ReciepientName { get; set; }
        public string ReciepientProfilePicture { get; set; }
        public string DeviceId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        //public NipFundTransferRequest NipFundTransferRequest { get; set; }
    }

    public class TransferCommandHandler : IRequestHandler<TransferCommand, Result>
    {
        private readonly IAPIClientService _apiClient;
        private readonly IApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly IAuthService _authService;
        public TransferCommandHandler(IAPIClientService aPIClientService, IApplicationDbContext context, 
            IUtilityService utilityService, IConfiguration configuration,INotificationService notificationService,
            IAuthService authService)
        {
            _apiClient = aPIClientService;
            _context = context;
            _utilityService = utilityService;
            _configuration = configuration;
            _notificationService = notificationService;
            _authService = authService;
        }


        public async Task<Result> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            string message = "";
            try
            {
                var wallet = await _context.Accounts.Include(x=>x.Product).FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Product.ProductCategory == Domain.Enums.ProductCategory.Cash);
                var transferRequest = new NipFundTransferRequest
                {
                    beneficiaryAccountName = request.beneficiaryAccountName,
                    beneficiaryBank = request.beneficiaryBank,
                    beneficiaryAccountNumber = request.beneficiaryAccountNumber,
                    currencyCode = request.currencyCode,
                    narration = request.narration,
                    sourceAccountName = wallet.AccountNumber,
                    transactionAmount = request.transactionAmount,
                    transactionReference = _utilityService.GenerateTransactionReference(),
                    userName = _configuration["Providus:userName"],
                    password = _configuration["Providus:password"]
                };
                var result = await _apiClient.NipFundTransfer(transferRequest);
                if (result.responseCode == "00")
                {
                    var createWalletTransactionCommand = new CreateWalletTransationCommand
                    {
                        TransactionStatus = TransactionStatus.Success,
                        Balance = wallet.ClosingBalance - Convert.ToDecimal(transferRequest.transactionAmount),
                        Description = transferRequest.narration,
                        TransactionAmount = Convert.ToDecimal(transferRequest.transactionAmount),
                        TransactionType = request.TransactionType,
                        UserId = request.UserId,
                        WalletAccountNo = wallet.AccountNumber,
                        WalletId = wallet.Id,
                        ReciepientName = request.ReciepientName,
                        ReciepientProfilePicture = request.ReciepientProfilePicture,
                        DeviceId=request.DeviceId
                    };
                    var handler = await new CreateWalletTransationCommandHandler(_context, _authService).Handle(createWalletTransactionCommand, cancellationToken);
                    //we need to implement logging
                    message = $"You sent {request.transactionAmount} to {request.ReciepientName} is pending";
                    var notification = new Notification
                    {
                        ApplicationType = request.ApplicationType,
                        NotificationStatus = NotificationStatus.Unread,
                        Message = message,
                        Status = Status.Active,
                        DeviceId = request.DeviceId,
                        UserId = request.UserId,
                        CreatedBy = request.UserId
                    };
                    await _notificationService.SendNotification(request.DeviceId, message);
                    await _context.Notifications.AddAsync(notification);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result.Success(result);
                }
                return Result.Failure(string.Concat("Transaction failed", ":", result.responseMessage));
            }
            catch (Exception ex)
            {
                return Result.Failure(string.Concat("Transaction failed", ":", ex.Message??ex.InnerException.Message));
            }
        }
    }
}
 