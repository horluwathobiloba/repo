using Microsoft.Extensions.Configuration;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Infrastructure.Utility
{
   public class VerificationEmailService : IVerificationEmailService
    {
        public async Task SendVerificationEmail(User user, IEmailService  emailService,IConfiguration configuration,IStringHashingService stringHashingService )
        {
            string webDomain = configuration["WebDomain"];
            var hashValue = (user.Email + DateTime.Now).ToString();
            user.Token = stringHashingService.CreateMD5StringHash(hashValue);
            var email = new EmailVm
            {
                Application = configuration["ApplicationName"],
                Subject = "User Verification",
                BCC = "",
                CC = "",
                Text = "",
                RecipientEmail = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Body = "We noticed your account has not been verified",
                Body1 = "Click the button below to verify your account.",
                ButtonText = "Verify Your Account",
                ButtonLink = webDomain + $"login?email={user.Email}&token={user.Token}'"

            };
            await emailService.EmailVerification(email);
        }
    }
}
