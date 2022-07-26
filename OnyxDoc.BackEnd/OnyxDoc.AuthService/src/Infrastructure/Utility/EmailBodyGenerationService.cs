using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OnyxDoc.AuthService.Infrastructure.Utility
{
    public class EmailBodyGenerationService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        public EmailBodyGenerationService(IHostingEnvironment environment,  IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }
        //This is a sample code for generating email body
        public string GenerateEmailBody()
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];
            string filePath = directory + "\\user_create_email.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/user_create_email.html";
            }

            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[subject]", "");
            mailText = mailText.Replace("[recipientEmail]", "");
            mailText = mailText.Replace("[body]", "");
            mailText = mailText.Replace("[body1]", "");
            mailText = mailText.Replace("[buttonText]", "");
            mailText = mailText.Replace("[buttonLink]", "");
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());
            return mailText;
        }
    }
}
