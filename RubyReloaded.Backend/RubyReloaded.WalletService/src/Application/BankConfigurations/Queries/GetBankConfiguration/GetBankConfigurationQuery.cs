using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankConfigurations.Queries.GetBankConfiguration
{
    public class GetBankConfigurationQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetBankConfigurationQueryHandler : IRequestHandler<GetBankConfigurationQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetBankConfigurationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetBankConfigurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.BankConfigurations.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Bank Configurationuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
