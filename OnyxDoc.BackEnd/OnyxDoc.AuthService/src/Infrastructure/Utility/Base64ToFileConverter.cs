using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public class Base64ToFileConverter : IBase64ToFileConverter
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IBlobStorageService _blobStorageService;

        public Base64ToFileConverter(IHostingEnvironment environment, IConfiguration configuration, IBlobStorageService blobStorageService)
        {
            _environment = environment;
            _configuration = configuration;
            _blobStorageService = blobStorageService;
        }
        public async Task<string> ConvertBase64StringToFile(string base64String, string fileName)
        {
           
            try
            {
                string filePath = null;
                if (!string.IsNullOrWhiteSpace(base64String))
                {
                    if (base64String.Contains("https://"))
                    {
                        return base64String;
                    }

                    if (!string.IsNullOrEmpty(base64String))
                    {
                        /*var newFileName = Convert.FromBase64String(base64String);
                        string fileStr = Encoding.Default.GetString(newFileName);*/
                        if (Convert.FromBase64String(base64String) == null)
                        {
                            return null;
                        }
                        var directory = _environment.WebRootPath;
                        if (directory == null)
                        {
                            directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot");
                        }
                        //Kubernetes
                        if (base64String.Contains(_environment.ContentRootPath + "wwwroot/images/"))
                        {
                            return base64String;
                        }
                        var uploads = Path.Combine(directory, "images");
                        bool exists = Directory.Exists(uploads);
                        if (!exists)
                            Directory.CreateDirectory(uploads);

                        var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);

                        var imageBytes = Convert.FromBase64String(base64String);
                        if (imageBytes.Length > 0)
                        {

                            filePath = await _blobStorageService.UploadFileToBlobAsync(fileName, imageBytes, "image/png");
                        }
                    }

                }
                return filePath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //[Obsolete]
        //public string RetrieveFileUrl(string fileName)
        //{
        //    if (!string.IsNullOrWhiteSpace(fileName))
        //    {
        //        var directory = _configuration["FileLocation:Images"];
        //        if (fileName.Contains(_configuration["FileLocation:Images"]))
        //        {
        //            return fileName;
        //        }
        //        if (fileName.Contains(_environment.ContentRootPath + "wwwroot/images/"))
        //        {
        //            return fileName;
        //        }
        //        fileName = fileName.Replace("\\images\\", "");
        //        fileName = fileName.Replace("C:\\inetpub\\wwwroot\\Ruby.LoanService\\wwwroot", "");
        //        return directory + fileName;
        //    }
        //    return "";
        //}
    }
}
