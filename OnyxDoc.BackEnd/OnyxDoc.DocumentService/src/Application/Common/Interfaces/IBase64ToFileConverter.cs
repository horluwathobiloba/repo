using Microsoft.AspNetCore.Hosting;

namespace OnyxDoc.DocumentService.Application.Common.Interfaces
{
    public interface IBase64ToFileConverter
    {
        string ConvertBase64StringToFile(string base64String, string fileName);
        string RetrieveFileUrl(string fileName);
    }
}
