using Microsoft.AspNetCore.Http;
using Onyx.ContractService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.ContractService.Application.Common.Models
{
    public class FileUploadRequest
    {
        private readonly IBlobStorageService _blobService;
        public FileUploadRequest()
        {
        }
        public FileUploadRequest(IBlobStorageService blobService)
        {
            _blobService = blobService;
        }

        public int DocumentId { get; set; }
        public string FileBase64String { get; set; }
        public string FileMimeType { get; set; }
        public string FileExtension { get; set; }
        internal string FileName { get; set; }
        internal string BlobUrl { get; set; }

        internal async Task<string> SaveBlobFile(string fileName, IBlobStorageService blobService)
        {
            if (string.IsNullOrEmpty(this.FileBase64String))
            {
                return null;
            }
            else if(!string.IsNullOrEmpty(this.FileBase64String) && this.FileBase64String.Contains("https"))
            {
                return FileBase64String;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(this.FileExtension) )
                {
                    throw new Exception("File extension for the file you are trying to upload must be specified.");
                }
                if (string.IsNullOrWhiteSpace(this.FileMimeType))
                {
                    throw new Exception("File mime type for the file you are trying to upload must be specified.");
                }
            }
            this.FileName = fileName + "_" + DateTime.Now.Ticks + "." + this.FileExtension;
            var fileBytes = Convert.FromBase64String(this.FileBase64String);
            this.BlobUrl = await blobService.UploadFileToBlobAsync(this.FileName, fileBytes, this.FileMimeType);

            return this.BlobUrl;
            ;
        }

        internal async Task<string> SaveBlobFile(IFormFile file, IBlobStorageService blobService)
        {
            if ( file.Length <= 0)
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(this.FileBase64String) && this.FileBase64String.Contains("https"))
            {
                return FileBase64String;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(file.ContentType))
                {
                    throw new Exception("File extension for the file you are trying to upload must be specified.");
                }
                if (string.IsNullOrWhiteSpace(this.FileMimeType))
                {
                    throw new Exception("File mime type for the file you are trying to upload must be specified.");
                }
            }  

            this.BlobUrl = await blobService.UploadFileToBlobAsync(file);

            return this.BlobUrl;
            ;
        }
    }
}
