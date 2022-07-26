using MediatR;
using Microsoft.Extensions.Configuration;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks
{
    public class GetAllProvidusBanksQuery:IRequest<Result>
    {

    }

    public class GetAllProvidusBanksQueryHandler : IRequestHandler<GetAllProvidusBanksQuery, Result>
    {
        private readonly IAPIClientService _apiClient;
        private readonly IConfiguration _configuration;
        public GetAllProvidusBanksQueryHandler(IAPIClientService aPIClientService,IConfiguration configuration)
        {
            _apiClient = aPIClientService;
            _configuration = configuration;
        }
        public async Task<Result> Handle(GetAllProvidusBanksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _apiClient.GetProvidusBanks();
                if (result is null)
                {
                    return Result.Failure("Retrieving Bank List from Providus was not successful");
                }
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Bank List from Providus was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
