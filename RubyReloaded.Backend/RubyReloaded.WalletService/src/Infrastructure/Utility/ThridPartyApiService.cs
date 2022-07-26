using RubyReloaded.WalletService.Application.Common.Interfaces;

namespace RubyReloaded.WalletService.Infrastructure.Utility
{

    public class ThridPartyApiService//: IThridPartyApiService
    {
        private readonly IAPIClientService _apiClient;
        public ThridPartyApiService(IAPIClientService aPIClient)
        {
            _apiClient = aPIClient;
        }

        
    }
}
