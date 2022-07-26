using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks;
using RubyReloaded.WalletService.Domain.ViewModels;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IAPIClientService
    {
        Task<string> Get(APIRequestDto request);
        Task<T> Get<T>(APIRequestDto request);
        Task<string> Post(APIRequestDto request);
        Task<T> Post<T>(APIRequestDto request);
        Task<string> PostAPIUrl(string apiUrl, string apiKey, object requestObject, bool isFormData);
        Task<DynamicAccountNumberResponseDto> CreateDynamicAccountNumber(string requestObject);
        Task<NipFundTransferResponse> NipFundTransfer(NipFundTransferRequest nipFundTransferRequest);
        Task<GetProvidusAccountResponse> GetWalletBalance(GetProvidusAccountRequest getProvidusAccountRequest);
        Task<GetAllProvidusBankResponse> GetProvidusBanks();
    }
}
