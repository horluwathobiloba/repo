using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Common.Interfaces
{
   public interface IBearerTokenService
    {
        Task<string> GetBearerToken(string apiUrl, object requestObject);
    }
}
