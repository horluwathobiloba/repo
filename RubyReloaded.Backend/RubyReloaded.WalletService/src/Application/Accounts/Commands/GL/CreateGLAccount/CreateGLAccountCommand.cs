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

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
   

    public class CreateGLAccountCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public AccountType GLAccountType { get; set; }
        public int ParentAccountId { get; set; }
        public string CurrencyCode { get; set; }
        public AccountClass GLAccountClass { get; set; }
        public int GLAccountPrefix { get; set; }
        public int ProductId { get; set; }
    }

    public class CreateGLAccountCommandHandler : IRequestHandler<CreateGLAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       

        public CreateGLAccountCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           
        }
        public async Task<Result> Handle(CreateGLAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {  
                var existingGLAccount = await _context.Accounts.Where(a => a.Name == request.Name
                                      && a.AccountType == request.GLAccountType && a.AccountClass == request.GLAccountClass
                                      && a.CurrencyCode != request.CurrencyCode).FirstOrDefaultAsync();
                if (existingGLAccount != null)
                {
                    return Result.Failure(new string[] { "GL Account already exist" });
                }
                var handler = new CreateAccountCommandHandler(_context, _aPIClient);
                var command = new CreateAccountCommand
                {
                    Name = request.Name,
                    ProductId = request.ProductId,
                    AccountClass = request.GLAccountClass,
                    CurrencyCode = request.CurrencyCode,
                    ParentAccountId = request.ParentAccountId,
                    UserId = request.UserId,
                    AccountType = request.GLAccountType,
                };

                var accountResponse = await handler.Handle(command, cancellationToken);
                if (accountResponse.Entity == null)
                {

                    return Result.Failure(string.IsNullOrEmpty(accountResponse.Message) ? accountResponse.Messages : new string[] { accountResponse.Message });
                }

                return Result.Success("GL Account was created successfully", accountResponse.Entity);

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "GL Account creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}



