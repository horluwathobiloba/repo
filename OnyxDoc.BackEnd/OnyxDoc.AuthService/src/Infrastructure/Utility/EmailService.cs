using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Utility
{
    public class EmailService : IEmailService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IAPIClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _context;

        public EmailService(IHostingEnvironment environment, IAPIClient apiClient, IConfiguration configuration,
           IApplicationDbContext context)
        {
            _environment = environment;
            _apiClient = apiClient;
            _configuration = configuration;
            _context = context;
        }



        public async Task<string> SubscriberSignUp(EmailVm emailVm)
        {
            var branding = await _context.Brandings.Where(b => b.SubscriberId == emailVm.SubscriberId && b.Status == Domain.Enums.Status.Active).FirstOrDefaultAsync();

           
            
            //send verification email
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];

            string filePath = directory + "\\subscriber_create.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/subscriber_create.html";
            }
            
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();


            if (branding != null)
            {
               
                mailText = mailText.Replace("#4e33ff", string.IsNullOrWhiteSpace(branding.ThemeColorCode)? "#4e33ff":branding.ThemeColorCode);
                mailText = mailText.Replace("https://reventfileshare.blob.core.windows.net/uploads/onyxlogo.png", string.IsNullOrWhiteSpace(branding.Logo) ? "https://reventfileshare.blob.core.windows.net/uploads/onyxlogo.png" : branding.Logo);
            }
           
            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[name]", emailVm.SubscriberName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[text]", emailVm.Text);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());
            //mailText = mailText.Replace("[appName]", "0nyx");

            string apiUrl = _configuration["Email:Url"];
            var requestBody = new
            {
                organizationId = 1,
                application = _configuration["ApplicationName"],
                // application = emailVm.Application,
                subject = emailVm.Subject,
                body = mailText,
                recipientMail = emailVm.RecipientEmail,
                recipientName = emailVm.RecipientName
            };
            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }

        public async Task<string> EmailVerification(EmailVm emailVm)
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

            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[firstName]", emailVm.FirstName);
            mailText = mailText.Replace("[lastName]", emailVm.LastName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[password]", emailVm.Password);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[body1]", emailVm.Body1);
            mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
            mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
            mailText = mailText.Replace("[appName]", appName);
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


            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }

        public async Task<string> AdminEmailVerification(EmailVm emailVm)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];
            string filePath = directory + "\\admin_setup_mail.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/admin_setup_mail.html";
            }


            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[firstName]", emailVm.FirstName);
            mailText = mailText.Replace("[lastName]", emailVm.LastName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[password]", emailVm.Password);
            mailText = mailText.Replace("[subscriberName]", emailVm.SubscriberName);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[body1]", emailVm.Body1);
            mailText = mailText.Replace("[body2]", emailVm.Body2);
            mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
            mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());



            string apiUrl = _configuration["Email:Url"];
            var requestBody = new
            {
                organizationId = 1,
                application = emailVm.Application,
                subject = emailVm.Subject,
                body = mailText,
                recipientMail = emailVm.RecipientEmail,
                recipientName = emailVm.RecipientName,
                buttonText = emailVm.ButtonText,
                buttonLink = emailVm.ButtonLink
            };


            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }

        public async Task<string> SuccessfullVerification(EmailVm emailVm)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];

            string filePath = directory + "\\success_verification.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/success_verification.html";
            }
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[firstName]", emailVm.RecipientName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());

            string apiUrl = _configuration["Email:Url"];
            var requestBody = new
            {
                organizationId = 1,
                application = emailVm.Application,
                subject = emailVm.Subject,
                body = mailText,
                recipientMail = emailVm.RecipientEmail
            };

            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }


        public async Task<string> SendForgotPasswordEmailAsync(EmailVm emailVm)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];

            string filePath = directory + "\\forgot_password.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/forgot_password.html";
            }

            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[firstName]", emailVm.FirstName);
            mailText = mailText.Replace("[lastName]", emailVm.LastName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[body1]", emailVm.Body1);
            mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
            mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());



            //Old implementation
            //mailText = mailText.Replace("[name]", emailVm.RecipientName);
            //mailText = mailText.Replace("[button]", emailVm.ButtonText);
            //mailText = mailText.Replace("[appName]", "0nyx");
            string apiUrl = _configuration["Email:Url"];
            var requestBody = new
            {

                organizationId = 1,
                application = _configuration["ApplicationName"],
                subject = emailVm.Subject,
                body = mailText,
                recipientMail = emailVm.RecipientEmail,
                buttonText = emailVm.ButtonText,
                buttonLink = emailVm.ButtonLink


                //SubscriberId = 1,
                //application = emailVm.Application,
                //subject = emailVm.Subject,
                //body = mailText,
                //recipientMail = emailVm.RecipientEmail
            };

            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }




        public async Task<string> ResetPasswordEmailAsync(EmailVm emailVm)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];

            string filePath = directory + "\\reset_password_email.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/reset_password_email.html";
            }

            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[firstName]", emailVm.FirstName);
            mailText = mailText.Replace("[lastName]", emailVm.LastName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[body1]", emailVm.Body1);
            mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
            mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());

            //Old implementation


            //var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");

            //string filePath = directory + "\\reset_Password.html";
            //StreamReader str = new StreamReader(filePath);
            //string mailText = str.ReadToEnd();
            //str.Close();
            //mailText = mailText.Replace("[name]", emailVm.RecipientName);
            //mailText = mailText.Replace("[FirstName]", emailVm.FirstName);
            //mailText = mailText.Replace("[LastName]", emailVm.LastName);
            //mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            //mailText = mailText.Replace("[appName]", "0nyx");

            string apiUrl = _configuration["Email:Url"];
            var requestBody = new
            {

                organizationId = 1,
                application = _configuration["ApplicationName"],
                subject = emailVm.Subject,
                body = mailText,
                recipientMail = emailVm.RecipientEmail,
                buttonText = emailVm.ButtonText,
                buttonLink = emailVm.ButtonLink


                //old implememntation

                //SubscriberId = 1,
                //application = emailVm.Application,
                //subject = emailVm.Subject,
                //body = mailText,
                //recipientMail = emailVm.RecipientEmail,
            };

            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }

        public async Task<string> ChangePasswordEmail(EmailVm emailVm)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:AppName"];

            string filePath = directory + "\\change_password.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/change_password.html";
            }
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[subject]", emailVm.Subject);
            mailText = mailText.Replace("[firstName]", emailVm.FirstName);
            mailText = mailText.Replace("[lastName]", emailVm.LastName);
            mailText = mailText.Replace("[recipientEmail]", emailVm.RecipientEmail);
            mailText = mailText.Replace("[body]", emailVm.Body);
            mailText = mailText.Replace("[body1]", emailVm.Body1);
            mailText = mailText.Replace("[buttonText]", emailVm.ButtonText);
            mailText = mailText.Replace("[buttonLink]", emailVm.ButtonLink);
            mailText = mailText.Replace("[appName]", appName);
            mailText = mailText.Replace("[year]", DateTime.Now.Year.ToString());


            //Old implementation
            //var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");

            //string filePath = directory + "\\change_password.html";
            //StreamReader str = new StreamReader(filePath);
            //string mailText = str.ReadToEnd();
            //str.Close();
            //mailText = mailText.Replace("[subject]", emailVm.Subject);
            //mailText = mailText.Replace("[name]", emailVm.RecipientName);
            //mailText = mailText.Replace("[text]", emailVm.Text);
            //mailText = mailText.Replace("[body1]", emailVm.Body);
            //mailText = mailText.Replace("[button]", emailVm.ButtonText);
            //mailText = mailText.Replace("[appName]", "0nyx");

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


                //old implementation

                //SubscriberId = 1,
                //application = emailVm.Application,
                //subject = emailVm.Subject,
                //body = mailText,
                //recipientMail = emailVm.RecipientEmail,
                //recipientName = emailVm.RecipientName
            };

            return await _apiClient.PostAPIUrl(apiUrl, "", requestBody, true);
        }

        public async Task<string> SendBulkInviteEmail(List<EmailVm> emailVms)
        {
            var directory = _environment.ContentRootPath.Replace("bin\\Debug\\netcoreapp3.1", "wwwroot\\Templates\\Email");
            string appName = _configuration["Email:BulkEmailUrl"];

            string filePath = directory + "\\user-invite.html";
            if (!File.Exists(filePath))
            {
                directory = _environment.ContentRootPath + "/wwwroot/Templates/Email";
                filePath = directory + "/user-invite.html";
            }
            //string apiUrl = _configuration["Email:BulkEmailUrl"];
            var list = new List<dynamic>();

            foreach (var emailVm in emailVms)
            {
                StreamReader str = new StreamReader(filePath);
                string mailText = str.ReadToEnd();
                str.Close();
                if (emailVm != null)
                {

                    mailText = mailText.Replace("[subject]", emailVm.Subject);
                    mailText = mailText.Replace("[firstName]", emailVm.FirstName);
                    mailText = mailText.Replace("[lastName]", emailVm.LastName);
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

            return await _apiClient.PostAPIUrl(appName, "", new { emailMessages = list }, true);
        }
    }
}
