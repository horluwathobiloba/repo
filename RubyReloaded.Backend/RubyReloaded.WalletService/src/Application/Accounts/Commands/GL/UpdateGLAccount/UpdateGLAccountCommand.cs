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

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
   

    public class UpdateGLAccountCommand : IRequest<Result>
    {
        public int GLAccountId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public AccountType GLAccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public int ParentGLAccountId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal DebitBalanceLimit { get; set; }
        public decimal CreditBalanceLimit { get; set; }
        public AccountFreezeType GLAccountFreezeType { get; set; }
        public AccountClass GLAccountClass { get; set; }
        public int GLAccountPrefix { get; set; }
        public int ProductId { get; set; }
    }

    public class UpdateGLAccountCommandHandler : IRequestHandler<UpdateGLAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       

        public UpdateGLAccountCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           
        }
        public async Task<Result> Handle(UpdateGLAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingGLAccount = await _context.Accounts.Where(a => a.Id == request.GLAccountId).FirstOrDefaultAsync();
                if (existingGLAccount == null)
                {
                    return Result.Failure(new string[] { "Invalid GLAccount Details" });
                }

                existingGLAccount.AccountClass = request.GLAccountClass;
                existingGLAccount.AccountFreezeType = request.GLAccountFreezeType;
                existingGLAccount.AccountPrefix = request.GLAccountPrefix;
                existingGLAccount.ProductId = request.ProductId;
                existingGLAccount.AccountFreezeType = request.GLAccountFreezeType;
                existingGLAccount.AccountType = request.GLAccountType;
                existingGLAccount.ClosingBalance = request.ClosingBalance;
                existingGLAccount.OpeningBalance = request.OpeningBalance;
                existingGLAccount.LastModifiedDate = DateTime.Now;
                existingGLAccount.LastModifiedBy = request.UserId;
                existingGLAccount.Name = request.Name;
                existingGLAccount.ParentAccountId = request.ParentGLAccountId;
                existingGLAccount.Status = Status.Active;
                existingGLAccount.StatusDesc = Status.Active.ToString();
                 _context.Accounts.Update(existingGLAccount);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("GL Account updated successfully",existingGLAccount);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "GL Account update was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}



