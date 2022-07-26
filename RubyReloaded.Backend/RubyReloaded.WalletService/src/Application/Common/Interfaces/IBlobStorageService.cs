using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
        void DeleteBlobData(string fileUrl);
        Task<string> UploadFileToBlobAsync(IFormFile file);
    }
}
