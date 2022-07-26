using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public class TransactionService : ITransactionService
    {
        private readonly IBearerTokenService _bearerTokenService;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _apiClient;

        public TransactionService(IConfiguration configuration, IBearerTokenService bearerTokenService, IApplicationDbContext context, IAPIClientService apiClient)
        {
            _bearerTokenService = bearerTokenService;
            _configuration = configuration;
            _apiClient = apiClient;

        }

        public async Task<string> CreateTransaction(TransactionRequest transactionRequest, CancellationToken cancellationToken)
        {
            try
            {
                //create the debit and credit leg entries
                if (transactionRequest != null)
                {
                    switch (transactionRequest.TransactionCategory)
                    {
                        case Domain.Enums.TransactionServiceType.FundWallet:
                            List<Transaction> transactions = new List<Transaction>();
                            var creditTransaction = new Transaction
                            {
                                TransactionCategory = transactionRequest.TransactionCategory,
                                TransactionCategoryDesc = transactionRequest.TransactionCategory.ToString(),
                                AccountId = transactionRequest.AccountId,
                                AccountNumber = transactionRequest.AccountNumber,
                                CreatedBy = transactionRequest.CustomerId,
                                CreatedByEmail = transactionRequest.Email,
                                Narration = transactionRequest.Narration,
                                Amount = transactionRequest.Amount,
                                CurrencyCode = transactionRequest.CurrencyCode,
                                CurrencyCodeDesc = transactionRequest.CurrencyCode.ToString(),
                                CreatedDate = DateTime.Now,
                                ExternalTransactionReference = transactionRequest.ExternalTransactionReference,
                                PaymentChannelId = transactionRequest.PaymentChannelId,
                                UserId = transactionRequest.CustomerId,
                                Status = Domain.Enums.Status.Active,
                                StatusDesc = Domain.Enums.Status.Active.ToString(),
                                TransactionType = transactionRequest.TransactionType,
                                TransactionTypeDesc = transactionRequest.TransactionType.ToString(),
                                TransactionDate = DateTime.Now,
                                ValueDate = DateTime.Now,
                                TransactionStatus = transactionRequest.TransactionStatus,
                                TransactionStatusDesc = transactionRequest.TransactionStatus.ToString(),
                                TransactionReference = transactionRequest.TransactionReference
                            };
                            transactions.Add(creditTransaction);
                            var debitTransaction = new Transaction
                            {
                                TransactionCategory = transactionRequest.TransactionCategory,
                                TransactionCategoryDesc = transactionRequest.TransactionCategory.ToString(),
                                AccountId = transactionRequest.AccountProductId,
                                AccountNumber = transactionRequest.AccountProductNumber,
                                CreatedBy = transactionRequest.CustomerId,
                                CreatedByEmail = transactionRequest.Email,
                                Narration = transactionRequest.Narration,
                                Amount = transactionRequest.Amount,
                                CurrencyCode = transactionRequest.CurrencyCode,
                                CurrencyCodeDesc = transactionRequest.CurrencyCode.ToString(),
                                CreatedDate = DateTime.Now,
                                ExternalTransactionReference = transactionRequest.ExternalTransactionReference,
                                PaymentChannelId = transactionRequest.PaymentChannelId,
                                UserId = transactionRequest.CustomerId,
                                Status = Domain.Enums.Status.Active,
                                StatusDesc = Domain.Enums.Status.Active.ToString(),
                                TransactionType = transactionRequest.TransactionType,
                                TransactionTypeDesc = transactionRequest.TransactionType.ToString(),
                                TransactionDate = DateTime.Now,
                                ValueDate = DateTime.Now,
                                TransactionStatus = transactionRequest.TransactionStatus,
                                TransactionStatusDesc = transactionRequest.TransactionStatus.ToString(),
                                TransactionReference = transactionRequest.TransactionReference
                            };
                            transactions.Add(debitTransaction);
                            await _context.Transactions.AddRangeAsync(transactions);
                            break;
                        case Domain.Enums.TransactionServiceType.Withdrawal:
                            break;
                        case Domain.Enums.TransactionServiceType.Transfers:
                            break;
                        case Domain.Enums.TransactionServiceType.BillPayment:
                            break;
                        default:
                            break;

                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return await Task.Run(() => "Transaction was successful");
            }
            catch (System.Exception ex)
            {
                return "error";
                throw ex;
            }
        }

     
    }
}
