using MediatR;

using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Commands
{
    public class CreateWalletCommand:IRequest<Result>
    {
        public Domain.Enums.ProductCategory ProductCategory { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string AccountName { get; set; }
    }

    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Result>
    {
        private readonly IAPIClientService _apiClient;
        private readonly IApplicationDbContext _context;
        public CreateWalletCommandHandler(IAPIClientService aPIClientService,IApplicationDbContext context)
        {
            _apiClient = aPIClientService;
            _context = context;
        }
        public async Task<Result> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dynamicAccount = await _apiClient.CreateDynamicAccountNumber(request.AccountName);
                if (!dynamicAccount.requestSuccessful)
                {
                    return Result.Failure("Wallet Generation failed");
                }

                var wallet = new Wallet
                {
                    Balance = 0,
                    ClosingBalance = 0,
                    OpeningBalance = 0,
                    CreatedDate = DateTime.Now,
                    CreatedBy = request.UserId,
                    CreatedByEmail = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.UserName,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    Email = request.Email,
                    ProductCategory = request.ProductCategory,
                    VirtualAccountName = dynamicAccount.account_name,
                    VirtualAccountNumber = dynamicAccount.account_number,
                    WalletAccountNumber=dynamicAccount.account_number,
                    UserId = request.UserId
                };
                await _context.Wallets.AddAsync(wallet);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(wallet);
            }
            catch (Exception ex)
            {
                return Result.Failure("User creation was not successful"+ex?.Message ?? ex?.InnerException.Message );
            }
           
        }
    }
}
