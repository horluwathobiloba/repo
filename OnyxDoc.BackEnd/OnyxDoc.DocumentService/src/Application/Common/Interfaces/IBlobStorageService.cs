using OnyxDoc.DocumentService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using OnyxDoc.DocumentService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace OnyxDoc.DocumentService.Infrastructure.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
        void DeleteBlobData(string fileUrl);
        Task<string> UploadFileToBlobAsync(IFormFile file);
    }
}
