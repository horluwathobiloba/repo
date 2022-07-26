using System.Collections.Generic;
using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.Common;

namespace Upskillz_invoice_mgt_Domain.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(ICollection<MailRequest> mailRequest);
    }
}
