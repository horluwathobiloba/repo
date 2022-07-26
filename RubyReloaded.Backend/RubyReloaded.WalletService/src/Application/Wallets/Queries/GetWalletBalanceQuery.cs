using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Wallets.Commands.UpdateWalletBalance;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletBalanceQuery : IRequest<Result>
    {
        public string WalletAccountNumber { get; set; }
        public string UserId { get; set; }
    }

    public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _apiClient;
       
        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _configuration;
        public GetWalletBalanceQueryHandler(IAPIClientService apiClient, IApplicationDbContext context, IUtilityService utilityService, IConfiguration configuration)
        {
            _context = context;
            _apiClient = apiClient;
            _configuration = configuration;
            _utilityService = utilityService;
            
             

        }
        public async Task<Result> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.WalletAccountNumber == request.WalletAccountNumber);
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Account Number");
                }
                var updateCommand = new UpdateWalletBalanceCommand
                {
                    WalletAccountNumber=request.WalletAccountNumber,
                    UserId=request.UserId
                };
                var handler =await  new UpdateWalletBalanceCommandHandler(_apiClient, _context, _utilityService, _configuration).Handle(updateCommand, cancellationToken);
                
                return handler;
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Balance was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
