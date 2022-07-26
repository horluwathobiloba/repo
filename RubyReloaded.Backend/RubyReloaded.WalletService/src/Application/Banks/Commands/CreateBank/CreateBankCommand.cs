//using AutoMapper.Configuration;
using MediatR;
using Microsoft.Extensions.Configuration;
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

namespace RubyReloaded.WalletService.Application.Bank.Commands.CreateBank
{

    public class BankVM
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Code { get; set; }
        public bool IsPayoutBank { get; set; }
        public bool IsVirtualBank { get; set; }
        public string Currency { get; set; }
    }


    public class CreateBankCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public List<BankVM> BankDtos { get; set; }
    }

    public class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAPIClientService _aPIClient;
        private readonly IConfiguration _configuration;
        public CreateBankCommandHandler( IApplicationDbContext context, IAPIClientService aPIClient, IConfiguration configuration)
        {
            _context = context;
            _aPIClient = aPIClient;
            _configuration = configuration;
        }
        public async Task<Result> Handle(CreateBankCommand request, CancellationToken cancellationToken)
        {
            try
            {             
                await _context.BeginTransactionAsync();
                var banks = new List<Domain.Entities.Bank>();
                //get all banks
                var allBanks = await _context.Banks.ToDictionaryAsync(a => a.Name);

                foreach (var item in request.BankDtos)
                {
                    if (allBanks != null || allBanks.Count() > 0)
                    {
                        if (allBanks.TryGetValue(item.Name, out Domain.Entities.Bank bank))
                        {
                            if (bank != null)
                            {
                                if (request.BankDtos.Count() == 1)
                                {
                                    return Result.Failure(new string[] { "Bank already exist" });
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                    }


                    var newBank = new Domain.Entities.Bank
                    {
                        Name = item.Name,
                        ShortName = item.ShortName,
                        Code = item.Code,
                        CreatedBy = request.UserId,
                        IsVirtualBank = item.IsVirtualBank,
                        Currency = item.Currency,
                        IsPayoutBank = item.IsPayoutBank,
                        CreatedDate = DateTime.Now,
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active,
                    };
                    banks.Add(newBank);
                }

                await _context.Banks.AddRangeAsync(banks);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(banks);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Bank creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
