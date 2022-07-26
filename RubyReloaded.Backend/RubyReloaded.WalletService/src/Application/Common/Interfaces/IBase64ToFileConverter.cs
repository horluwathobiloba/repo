using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IBase64ToFileConverter
    {
        Task<string> ConvertBase64StringToFile(string base64String, string fileName);
        Task<string> RetrieveFileUrl(string fileName);
    }
}
