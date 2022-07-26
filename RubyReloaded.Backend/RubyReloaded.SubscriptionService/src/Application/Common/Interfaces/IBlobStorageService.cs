using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace RubyReloaded.SubscriptionService.Infrastructure.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
        void DeleteBlobData(string fileUrl);
        Task<string> UploadFileToBlobAsync(IFormFile file);
    }
}
