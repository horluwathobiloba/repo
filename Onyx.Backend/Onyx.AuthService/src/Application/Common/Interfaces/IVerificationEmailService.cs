using Microsoft.Extensions.Configuration;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Common.Interfaces
{
    public interface IVerificationEmailService
    {
        Task SendVerificationEmail(User user, IEmailService emailService, IConfiguration configuration, IStringHashingService stringHashingService);

    }
}
