using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigurations.Queries.GetVirtualAccountConfiguration
{
    public class GetVirtualAccountConfigurationByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int VirtualAccountConfigurationId { get; set; }
    }

    public class GetVirtualAccountConfigurationByIdQueryHandler : IRequestHandler<GetVirtualAccountConfigurationByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetVirtualAccountConfigurationByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetVirtualAccountConfigurationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.VirtualAccountConfigurations.FirstOrDefaultAsync(x => x.Id == request.VirtualAccountConfigurationId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Virtual Account Configurationuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
