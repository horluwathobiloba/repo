using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IBase64ToFileConverter
    {
        Task<string> ConvertBase64StringToFile(string base64String, string fileName);
       // string RetrieveFileUrl(string fileName);
    }
}
