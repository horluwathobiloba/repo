using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Onyx.WorkFlowService.Application.Common.Interfaces
{
    public class Base64ToFileConverter : IBase64ToFileConverter
    {


        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Base64ToFileConverter(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }
        public string ConvertBase64StringToFile(string base64String, string fileName)
        {
            var filePath = "";
            try
            {
                if (!string.IsNullOrEmpty(base64String))
                {
                    var directory = _environment.WebRootPath;
                    if (directory == null)
                    {
                        directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot");
                    }
                    var uploadDirectory = directory + "\\images\\";
                    //var uploadDirectory = configuration["FileLocation:FileWrite"];
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    var imageBytes = Convert.FromBase64String(base64String);
                    if (imageBytes.Length > 0)
                    {
                        string file = Path.Combine(uploadDirectory, fileName);
                        if (imageBytes.Length > 0)
                        {
                            using (var stream = new FileStream(file, FileMode.Create))
                            {
                                stream.Write(imageBytes, 0, imageBytes.Length);
                                stream.Flush();
                            }
                            filePath = fileName;
                        }
                    }
                }

                return filePath;
            }
            catch (Exception ex)
            {

                return "";
            }
        }


        [Obsolete]
        public string RetrieveFileUrl(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var directory = _configuration["FileLocation:Images"];
                fileName = fileName.Replace("\\images\\", "");
                fileName = fileName.Replace("C:\\inetpub\\wwwroot\\Onyx.LoanService\\wwwroot", "");
                return directory + fileName;
            }
            return "";
        }
    }

}
