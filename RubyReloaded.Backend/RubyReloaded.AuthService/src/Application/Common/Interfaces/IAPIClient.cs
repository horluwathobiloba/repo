using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Common.Interfaces
{
    public interface IAPIClient
    {
        Task<string> PostAPIUrl(string apiUrl, string apiKey, object requestObject, bool isFormData = false);
        Task<string> GetAPIUrl(string apiUrl, string apiKey, string referenceId);
    }
}
