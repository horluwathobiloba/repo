using RubyReloaded.WalletService.Domain.ViewModels;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IUtilityService
    {
        string GenerateTransactionReference();
    }
}
