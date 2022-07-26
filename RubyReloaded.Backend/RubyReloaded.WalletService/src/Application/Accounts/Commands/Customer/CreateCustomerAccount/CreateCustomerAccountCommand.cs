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
using RubyReloaded.WalletService.Application.Accounts.Commands.CreateAccount;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.CreateCustomerAccount
{
   

    public class CreateCustomerAccountCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CreateCustomerAccountCommandHandler : IRequestHandler<CreateCustomerAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       

        public CreateCustomerAccountCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           
        }
        public async Task<Result> Handle(CreateCustomerAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //get cash product id
                var cashProduct = await _context.Products.Where(a => a.ProductCategory == ProductCategory.Cash).FirstOrDefaultAsync();
                if (cashProduct == null)
                {
                    return Result.Failure( "Cash Product does not exist, Please contact support" );
                }
                var existingCustomerAccount = await _context.Accounts.Where(a=>a.CreatedBy == request.CustomerId && a.Name == request.Name 
                                      && a.AccountType == AccountType.Customer && a.AccountClass == AccountClass.Liability && a.CurrencyCode != request.CurrencyCode).FirstOrDefaultAsync();
                if (existingCustomerAccount != null)
                {
                    return Result.Failure(new string[] { "Customer Account Details already exist"});
                }
                var handler = new CreateAccountCommandHandler(_context, _aPIClient);
                var command = new CreateAccountCommand
                {
                    Name = request.Name,
                    ProductId = cashProduct.Id,
                    AccountType = AccountType.Customer,
                    AccountClass = AccountClass.Liability,
                    CurrencyCode = request.CurrencyCode,
                    ParentAccountId = cashProduct.GLSubClassAccountId,
                    UserId = request.CustomerId,
                };

                var accountResponse = await handler.Handle(command, cancellationToken);
                if (accountResponse.Entity == null)
                {

                    return Result.Failure(string.IsNullOrEmpty(accountResponse.Message) ? accountResponse.Messages : new string[] { accountResponse.Message });
                }

                return Result.Success("Customer Account was created successfully", accountResponse.Entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Customer Account creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}



