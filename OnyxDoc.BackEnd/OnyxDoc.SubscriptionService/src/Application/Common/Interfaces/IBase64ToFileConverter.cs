using Microsoft.AspNetCore.Hosting;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IBase64ToFileConverter
    {
        string ConvertBase64StringToFile(string base64String, string fileName);
        string RetrieveFileUrl(string fileName);
    }
}
