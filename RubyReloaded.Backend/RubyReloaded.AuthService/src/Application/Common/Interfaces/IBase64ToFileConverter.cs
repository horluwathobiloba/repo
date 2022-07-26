using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface IBase64ToFileConverter
    {
        Task<string> ConvertBase64StringToFile(string base64String, string fileName);
        Task<string> ConvertByteImageToFile(byte[] Image, string fileName);
        Task<string> ConvertByteToFile(byte[] uploadData, string fileName,string mimeType);

       // string RetrieveFileUrl(string fileName);
    }
}
