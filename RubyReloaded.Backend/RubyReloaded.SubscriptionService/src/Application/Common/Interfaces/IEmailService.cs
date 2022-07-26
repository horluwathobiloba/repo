using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendBulkEmail(List<EmailVm> emailVms);
        Task<string> SendEmail(EmailVm emailVm);
    }
}
