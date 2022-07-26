using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendBulkEmail(List<EmailVm> emailVms);
        Task<string> SendEmail(EmailVm emailVm);
    }
}
