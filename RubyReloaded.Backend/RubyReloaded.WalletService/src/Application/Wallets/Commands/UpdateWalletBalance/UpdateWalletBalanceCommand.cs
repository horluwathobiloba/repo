using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Commands.UpdateWalletBalance
{
    public class UpdateWalletBalanceCommand:IRequest<Result>
    {
        public string WalletAccountNumber { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateWalletBalanceCommandHandler : IRequestHandler<UpdateWalletBalanceCommand, Result>
    {
        private readonly IAPIClientService _apiClient;
        private readonly IApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _configuration;
        public UpdateWalletBalanceCommandHandler(IAPIClientService aPIClientService, IApplicationDbContext context, IUtilityService utilityService, IConfiguration configuration)
        {
            _apiClient = aPIClientService;
            _context = context;
            _utilityService = utilityService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(UpdateWalletBalanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //get wallet
                var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAccountNumber == request.WalletAccountNumber&&request.UserId==x.UserId);
                // Call the api
                var getAccountRequest = new GetProvidusAccountRequest { 
                
                   accountNumber=wallet.VirtualAccountNumber,
                    userName = _configuration["Providus:userName"],
                    password = _configuration["Providus:password"]
                };
                var responseDetails = await _apiClient.GetWalletBalance(getAccountRequest);
                if (responseDetails is null ||responseDetails.responseCode=="01")
                {
                    return Result.Failure("Wallet details could not be found");
                }

                wallet.Balance = Convert.ToDecimal(responseDetails.availableBalance);
                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(responseDetails);
            }
            catch (Exception ex)
            {
                return Result.Failure(string.Concat("Transaction failed",": ", ex?.Message ?? ex?.InnerException.Message));
            }
        }
    }
}
