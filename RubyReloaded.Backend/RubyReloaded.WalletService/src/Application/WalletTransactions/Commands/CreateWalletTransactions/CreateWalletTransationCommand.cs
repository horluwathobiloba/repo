using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.CreateVirtualAccountConfig
{
    public class CreateWalletTransationCommand : IRequest<Result>
    {
        public string UserId { get; set; }
       // public string AccountHolder { get; set; }
        public int PaymentChannelId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public int WalletId { get; set; }   
        public string WalletAccountNo { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string ReciepientName { get; set; }
        public string ReciepientProfilePicture { get; set; }
        public string AuthToken { get; set; }
        public string DeviceId { get; set; }

    }

    public class CreateWalletTransationCommandHandler : IRequestHandler<CreateWalletTransationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public CreateWalletTransationCommandHandler(IApplicationDbContext context,IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateWalletTransationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var walletTransationExist = await _context.WalletTransactions.FirstOrDefaultAsync(x => x.TransactionAmount == request.TransactionAmount &&
                x.WalletId == request.WalletId && x.CreatedDate.AddMinutes(1) > DateTime.Now);
                if (walletTransationExist != null)
                {
                    return Result.Failure(new string[] { "Duplicate Transaction" });
                }

                await _context.BeginTransactionAsync();

                var user = await _authService.GetUserById(request.AuthToken, request.UserId);
                if (user is null)
                {
                    return Result.Failure("Invalid User");
                }
                var walletTransaction = new WalletTransaction
                {
                    TransactionType = request.TransactionType,
                    TransactionAmount = request.TransactionAmount,
                    Description = request.Description,
                    WalletId = request.WalletId,
                    TransactionTypeDesc = request.TransactionType.ToString(),
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    StatusDesc = request.TransactionType.ToString(),
                    TransactionStatus = request.TransactionStatus,
                    UserId=request.UserId,
                    ReciepientName=request.ReciepientName,
                    ReciepientProfilePicture=request.ReciepientProfilePicture,
                    AccountHolder=user.entity.name,
                    PaymentChannelId=request.PaymentChannelId,
                    DeviceId=request.DeviceId
                };

                await _context.WalletTransactions.AddAsync(walletTransaction);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(walletTransaction);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Wallet Transaction creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
