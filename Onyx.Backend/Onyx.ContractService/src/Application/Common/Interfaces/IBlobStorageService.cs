using Onyx.ContractService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Onyx.ContractService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using Onyx.ContractService.Domain.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Onyx.ContractService.Infrastructure.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
        void DeleteBlobData(string fileUrl);
        Task<string> UploadFileToBlobAsync(IFormFile file);
    }
}
