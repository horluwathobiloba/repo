using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private IWebHostEnvironment Environment;

        public FileUploadService(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        public async Task<string> UploadFormFiles(List<IFormFile> postedFiles)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                { 
                    await postedFile.CopyToAsync(stream);
                }
            }

            return "";
        }
    }
}
