using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
 
namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces 
{
    public interface IAPIClient
    {
        Task<string> Get(string apiUrl, string apiKey, bool isFormData = false); 
        Task<T> Get<T>(string apiUrl, string apiKey, bool isFormData = false);
        Task<string> Post(string apiUrl, string apiKey, object requestObject, bool isFormData = false);
        Task<T> Post<T>(string apiUrl, string apiKey, object requestObject, bool isFormData = false);

        Task<T> JsonPost<T>(string apiUrl, string apiKey, object requestObject, bool isFormData = false);
    }
}
