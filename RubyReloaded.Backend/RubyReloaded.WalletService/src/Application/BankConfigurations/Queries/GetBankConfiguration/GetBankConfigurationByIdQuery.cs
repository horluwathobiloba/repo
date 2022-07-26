using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BankConfigurations.Queries.GetBankConfiguration
{
    public class GetBankConfigurationByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int BankConfigurationId { get; set; }
    }

    public class GetBankConfigurationByIdQueryHandler : IRequestHandler<GetBankConfigurationByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetBankConfigurationByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetBankConfigurationByIdQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var result = await _context.BankConfigurations.FirstOrDefaultAsync(x => x.Id == request.BankConfigurationId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Bank Configuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
