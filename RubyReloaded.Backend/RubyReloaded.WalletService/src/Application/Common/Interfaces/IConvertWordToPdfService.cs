using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IConvertWordToPdfService
    {
        Task<string> ConvertWordToPdf(string fileName);
    }
}
