using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionServiceType TransactionCategory { get; set; }
        public string TransactionCategoryDesc { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusDesc { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string TransactionReference { get; set; }
        public string ExternalTransactionReference { get; set; }
        public string RecipientName { get; set; }
        public string RecipientProfilePicture { get; set; }
        public int PaymentChannelId { get; set; }
        public PaymentChannel PaymentChannel { get; set; }
        public string Narration { get; set; }
        public string AuthToken { get; set; }
        public string DeviceId { get; set; }

    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;
        public CreateTransactionCommandHandler(IApplicationDbContext context,IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //verify because 1mins is a duplicate transaction; ask the business guys
                var transactionExists = await _context.Transactions.Include(x => x.Account).FirstOrDefaultAsync(x => x.Amount == request.Amount 
                && x.Account.AccountNumber == request.AccountNumber && x.AccountId == request.AccountId 
                && (x.CreatedDate ==  DateTime.Now || DateTime.Now.Subtract(x.CreatedDate).Minutes <= 60) );

                if (transactionExists != null)
                {
                    return Result.Failure(new string[] { "Duplicate Transaction" });
                }

                await _context.BeginTransactionAsync();

                var user = await _authService.GetUserById(request.AuthToken, request.UserId);
                if (user is null)
                {
                    return Result.Failure("Invalid User");
                }
                var transaction = new Transaction
                {
                    Amount = request.Amount,
                    Narration = request.Narration,
                    TransactionType = request.TransactionType,
                    TransactionTypeDesc = request.TransactionType.ToString(),
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    TransactionStatus = request.TransactionStatus,
                    TransactionStatusDesc = request.TransactionStatusDesc,
                    UserId = request.UserId,
                    AccountNumber = request.AccountNumber,
                    CreatedByEmail = request.UserId,
                    AccountId = request.AccountId,
                    CurrencyCode = request.CurrencyCode,
                    CurrencyCodeDesc = request.CurrencyCode.ToString(),
                    TransactionDate = DateTime.Now,
                    ValueDate = DateTime.Now,
                    Description = request.Narration,
                    ExternalTransactionReference = request.ExternalTransactionReference,
                    PaymentChannelId = request.PaymentChannelId,
                    TransactionCategory = request.TransactionCategory,
                    TransactionCategoryDesc = request.TransactionCategory.ToString(),
                    TransactionReference = request.TransactionReference,
                    DeviceId = request.DeviceId
                };

                await _context.Transactions.AddAsync(transaction);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Transaction created successfully", transaction);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Transaction creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
