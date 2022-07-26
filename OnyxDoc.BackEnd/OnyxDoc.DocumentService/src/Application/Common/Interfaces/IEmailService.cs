using OnyxDoc.DocumentService.Application.Common.Models;
using OnyxDoc.DocumentService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.DocumentService.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendBulkEmail(List<EmailVm> emailVms);
        Task<string> SendEmail(EmailVm emailVm);
    }
}
