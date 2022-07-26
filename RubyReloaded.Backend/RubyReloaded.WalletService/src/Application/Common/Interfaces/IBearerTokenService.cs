using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IBearerTokenService
    {
        Task<string> GetBearerToken(string apiUrl, object requestObject);
    }
}
