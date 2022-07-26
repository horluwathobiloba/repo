using RubyReloaded.WalletService.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendBulkEmail(List<EmailVm> emailVms);
        Task<string> SendEmail(EmailVm emailVm);
    }
}
