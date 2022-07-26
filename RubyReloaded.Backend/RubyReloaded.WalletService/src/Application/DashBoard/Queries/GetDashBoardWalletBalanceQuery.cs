using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Wallets.Commands.UpdateWalletBalance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.DashBoard.Queries
{
    public class GetDashBoardWalletBalanceQuery:IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetDashBoardWalletBalanceQueryHandler : IRequestHandler<GetDashBoardWalletBalanceQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _apiClient;

        private readonly IUtilityService _utilityService;
        private readonly IConfiguration _configuration;
        public GetDashBoardWalletBalanceQueryHandler(IAPIClientService apiClient, IApplicationDbContext context, IUtilityService utilityService, IConfiguration configuration)
        {
            _context = context;
            _apiClient = apiClient;
            _configuration = configuration;
            _utilityService = utilityService;

        }
        public async Task<Result> Handle(GetDashBoardWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.ProductCategory==Domain.Enums.ProductCategory.Cash);
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Account Number");
                }
                var updateCommand = new UpdateWalletBalanceCommand
                {
                    WalletAccountNumber = wallet.WalletAccountNumber,
                    UserId = request.UserId
                };
                var handler = await new UpdateWalletBalanceCommandHandler(_apiClient, _context, _utilityService, _configuration).Handle(updateCommand, cancellationToken);

                return handler;
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Balance was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
