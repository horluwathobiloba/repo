using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.Entities;
using Upskillz_invoice_mgt_Domain.IRepository.Base;

namespace Upskillz_invoice_mgt_Domain.IRepository
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task GetById(string id);

    }
}
