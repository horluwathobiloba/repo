using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
    public interface IVerificationEmailService
    {
        Task SendVerificationEmail(User user, IEmailService emailService, IConfiguration configuration, IStringHashingService stringHashingService);

    }
}
