using AutoMapper.Configuration;
using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.Customer.UpdateAccount
{
   

    public class UpdateCustomerAccountCommand : IRequest<Result>
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public int ParentAccountId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal DebitBalanceLimit { get; set; }
        public decimal CreditBalanceLimit { get; set; }
        public AccountFreezeType AccountFreezeType { get; set; }
        public AccountClass AccountClass { get; set; }
        public int AccountPrefix { get; set; }
        public int ProductId { get; set; }
    }

    public class UpdateCustomerAccountCommandHandler : IRequestHandler<UpdateCustomerAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       

        public UpdateCustomerAccountCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           
        }
        public async Task<Result> Handle(UpdateCustomerAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAccount = await _context.Accounts.Where(a => a.Id == request.AccountId).FirstOrDefaultAsync();
                if (existingAccount == null)
                {
                    return Result.Failure(new string[] { "Invalid Account Details" });
                }

                existingAccount.AccountClass = request.AccountClass;
                existingAccount.AccountFreezeType = request.AccountFreezeType;
                existingAccount.AccountPrefix = request.AccountPrefix;
                existingAccount.ProductId = request.ProductId;
                existingAccount.AccountFreezeType = request.AccountFreezeType;
                existingAccount.AccountType = request.AccountType;
                existingAccount.ClosingBalance = request.ClosingBalance;
                existingAccount.OpeningBalance = request.OpeningBalance;
                existingAccount.LastModifiedDate = DateTime.Now;
                existingAccount.LastModifiedBy = request.UserId;
                existingAccount.Name = request.Name;
                existingAccount.ParentAccountId = request.ParentAccountId;
                existingAccount.Status = Status.Active;
                existingAccount.StatusDesc = Status.Active.ToString();
                 _context.Accounts.Update(existingAccount);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Account updated successfully",existingAccount);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Account update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}



