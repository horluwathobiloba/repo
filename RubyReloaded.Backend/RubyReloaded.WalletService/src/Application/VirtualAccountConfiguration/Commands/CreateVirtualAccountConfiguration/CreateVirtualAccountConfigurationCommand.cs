using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enitities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.CreateVirtualAccountConfig
{
    public class CreateVirtualAccountConfigurationCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string BankId { get; set; }
        public string SettlementAccount { get; set; }
        public string Currency { get; set; }
    }

    public class CreateVirtualAccountConfigurationCommandHandler : IRequestHandler<CreateVirtualAccountConfigurationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public CreateVirtualAccountConfigurationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(CreateVirtualAccountConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isVirtualAccountConfigNameThere = await _context.VirtualAccountConfigurations.FirstOrDefaultAsync(x => x.BankId == request.BankId &&
                x.Currency == request.Currency && x.SettlementAccount == request.SettlementAccount);
                if (isVirtualAccountConfigNameThere != null)
                {
                    return Result.Failure(new string[] { "Virtual Account Configuration Details already Exit" });
                }
                await _context.BeginTransactionAsync();

                var virtualAccountConfig = new VirtualAccountConfiguration
                {
                    Name = request.BankId + "_" + request.Currency,
                    BankId = request.BankId,
                    Currency = request.Currency,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    StatusDesc = Status.Active.ToString(),
                    Status = Status.Active,
                };
                await _context.VirtualAccountConfigurations.AddAsync(virtualAccountConfig);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(virtualAccountConfig);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Virtual Account Configuration creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
