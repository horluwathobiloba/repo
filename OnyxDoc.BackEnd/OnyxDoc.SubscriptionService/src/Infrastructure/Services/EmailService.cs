﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Infrastructure.Utility
{
    public class EmailService : IEmailService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IAPIClient _aPIClient;

        public EmailService(IHostingEnvironment environment, IConfiguration configuration, IAPIClient aPIClient)
        {
            _environment = environment;
            _configuration = configuration;
            _aPIClient = aPIClient;
        }

        public async Task<string> SendEmail(EmailVm emailVm)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:ApplicationName"];
            string filePath = directory + "\\contract_request.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/contract_request.html";
            }

            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[firstname]", emailVm.FirstName);
            mailText = mailText.Replace("[lastname]", emailVm.LastName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[body1]", emailVm.Body1);
            mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
            mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[displayButton]", string.Concat('"', emailVm.DisplayButton, '"'));
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());
            string apiUrl = _configuration["Email:Url"];
            var requestBody = new
            {
                organizationId = 1,
                application = _configuration["ApplicationName"],
                subject = emailVm.Subject,
                body = mailText,
                recipientMail = emailVm.RecipientEmail,
                recipientName = emailVm.RecipientName,
                buttonText = emailVm.ButtonText,
                buttonLink = emailVm.ButtonLink
            };


            return await _aPIClient.Post(apiUrl, "", requestBody, true);
        }

        public async Task<string> SendBulkEmail(List<EmailVm> emailVms)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:ApplicationName"];
            string filePath = directory + "\\contract_request.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/contract_request.html";
            }

            

            string apiUrl = _configuration["Email:BulkEmailUrl"];
            var list = new List<dynamic>();

            foreach (var emailVm in emailVms)
            {
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                if (emailVm != null)
                {

                    mailText = mailText.Replace("[subject]", emailVm.Subject);
                    mailText = mailText.Replace("[firstname]", emailVm.FirstName);
                    mailText = mailText.Replace("[lastname]", emailVm.LastName);
                    mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
                    mailText = mailText.Replace("[body]", emailVm.Text);
                    mailText = mailText.Replace("[body1]", emailVm.Body);
                    mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
                    mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
                    mailText = mailText.Replace("[appName]", appName);
                    mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());
                    list.Add(new
                    {
                        organizationId = 1,
                        application = _configuration["ApplicationName"],
                        subject = emailVm.Subject,
                        body = mailText,
                        recipientMail = emailVm.RecipientEmail,
                        recipientName = emailVm.RecipientName,
                        buttonText = emailVm.ButtonText,
                        buttonLink = emailVm.ButtonLink
                    }); 
                }
            }

            return await _aPIClient.Post(apiUrl, "", new {emailMessages = list}, true);
        }
    }
}
