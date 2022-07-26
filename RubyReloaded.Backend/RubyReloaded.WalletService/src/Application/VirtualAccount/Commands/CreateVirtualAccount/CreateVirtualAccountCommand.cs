
using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccount.Commands.CreateVirtualAccount
{
    public class VirtualAccountDtos
    {
        public int WalletId { get; set; }
        public int BankId { get; set; }
        public string AccountNo { get; set; }

    }
    public class CreateVirtualAccountCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public List<VirtualAccountDtos> VirtualAccountDtos { get; set; }
    }

    public class CreateVirtualAccountCommandHandler : IRequestHandler<CreateVirtualAccountCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
       // private readonly IConfiguration _configuraton;

        public CreateVirtualAccountCommandHandler(IApplicationDbContext context, IAPIClientService aPIClient)
        {
            _context = context;
            _aPIClient = aPIClient;
           // _configuraton = configuration;
        }
        public async Task<Result> Handle(CreateVirtualAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var virtualaccounts = new List<Domain.Entities.VirtualAccount>();
                //get all banks
                var allAccounts = await _context.VirtualAccounts.ToDictionaryAsync(a => a.AccountNo);

                foreach (var item in request.VirtualAccountDtos)
                {
                    if (allAccounts != null || allAccounts.Count > 0)
                    {
                        if (allAccounts.TryGetValue(item.AccountNo, out Domain.Entities.VirtualAccount virtualaccount))
                        {
                            if (virtualaccount != null)
                            {
                                if (request.VirtualAccountDtos.Count == 1)
                                {
                                    return Result.Failure(new string[] { "Account already exist" });
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                    }


                    var newAccount = new Domain.Entities.VirtualAccount
                    {
                        WalletId = item.WalletId,
                        BankId = item.BankId,
                        AccountNo = item.AccountNo,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                    };
                    virtualaccounts.Add(newAccount);
                }

                await _context.VirtualAccounts.AddRangeAsync(virtualaccounts);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(virtualaccounts);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Bank creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}

