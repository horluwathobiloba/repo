using MediatR;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Commands.ProvidusFundTransferCommand
{
    public class ProvidusFundTransferCommand : IRequest<Result>
    {
        public string CreditAccount { get; set; }
        public string DebitAccount { get; set; }
        public string TransactionAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string Narration { get; set; }
        public string TransactionReference { get; set; }
        
    }


    public class ProvidusFundTransferCommandHandler : IRequestHandler<ProvidusFundTransferCommand, Result>
    {
        private readonly IAPIClientService _apiClient;
        private readonly IApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly IAuthService _authService;
        private readonly IProvidusBankService _providusBankService;
        public ProvidusFundTransferCommandHandler(IAPIClientService aPIClientService, IApplicationDbContext context,
            IUtilityService utilityService, IConfiguration configuration, INotificationService notificationService,
            IAuthService authService, IProvidusBankService providusBankService)
        {
            _apiClient = aPIClientService;
            _context = context;
            _utilityService = utilityService;
            _configuration = configuration;
            _notificationService = notificationService;
            _authService = authService;
            _providusBankService = providusBankService;
        }
        public async Task<Result> Handle(ProvidusFundTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var providusFundTransferRequest = new ProvidusFundTransferRequest
                {
                    creditAccount = request.CreditAccount,
                    debitAccount = request.DebitAccount,
                    currencyCode = request.CurrencyCode,
                    narration = request.Narration,
                    password = _configuration["Providus:password"],
                    transactionAmount = request.TransactionAmount,
                    transactionReference = _utilityService.GenerateTransactionReference(),
                    userName = _configuration["Providus:userName"],
                };
                var response = await _providusBankService.ProvidusFundTransfer(providusFundTransferRequest);
                if (response.ResponseCode!="00")
                {
                    return Result.Failure(response);

                }
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure(string.Concat("Transaction failed", ":", ex.Message ?? ex.InnerException.Message));
            }
        }
    }
}
