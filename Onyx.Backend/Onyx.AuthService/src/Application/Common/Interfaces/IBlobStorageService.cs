using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
   public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
        void DeleteBlobData(string fileUrl);
    }
}
