using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Common.Interfaces
{
   public interface IBearerTokenService
    {
        Task<string> GetBearerToken();
        Task<string> GetBearerToken(string apiUrl, object requestObject);
    }
}
