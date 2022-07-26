using AutoMapper.Configuration;
using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
/*using System.Data.Entity;*/
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.CreateAccount
{
   

    public class CreateAccountCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public AccountClass AccountClass { get; set; }
        public AccountType AccountType { get; set; }
        public int ParentAccountId { get; set; }
        public string CurrencyCode { get; set; }
        public int ProductId { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       

        public CreateAccountCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           
        }
        public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAccount = await _context.Accounts.Where(a=>a.ProductId == request.ProductId && a.Name == request.Name 
                                      && a.AccountType == request.AccountType && a.AccountClass == request.AccountClass
                                      && !string.IsNullOrWhiteSpace(a.AccountNumber)).FirstOrDefaultAsync();
                if (existingAccount != null)
                {
                    return Result.Failure("Account Details already exist");
                }

                var virtualAccount = await _aPIClient.CreateDynamicAccountNumber(request.Name);
                //Add virtualAccount property to the Account Class
                var account = new Domain.Entities.Account
                {
                    AccountClass = request.AccountClass,
                    AccountStatus =AccountStatus.Active,
                    AccountStatusDesc = AccountStatus.Active.ToString(),
                    AccountType = request.AccountType,
                    CurrencyCode = request.CurrencyCode,
                    Name = request.Name,
                    ParentAccountId = request.ParentAccountId,
                    ProductId = request.ProductId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active, 
                    AccountNumber=virtualAccount.account_number,
                    UserId=request.UserId
                };
                if (request.AccountType == AccountType.Customer || request.AccountType == AccountType.PGL || request.AccountType == AccountType.SPGL)
                {
                    //get product and set minimum and maximum funding amount , allow customer override is going to be someplace else
                    var product = await _context.Products.Where(a => a.Id == request.ProductId).FirstOrDefaultAsync();
                    if (product == null)
                    {
                        return Result.Failure("Invalid Product Details. Please contact Support !");
                    }
                    account.MinimumFundingAmount = product.MinimumFundingAmount;
                    account.MaximumFundingAmount = product.MaximumFundingAmount;
                    account.MinimumWithdrawalAmount = product.MinimumWithdrawalAmount;
                    account.MinimumFundingAmount = product.MinimumFundingAmount;
                }
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Account created successfully", account);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Account creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}



