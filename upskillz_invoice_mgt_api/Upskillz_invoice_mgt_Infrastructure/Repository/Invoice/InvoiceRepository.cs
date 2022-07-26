using System.Threading.Tasks;
using Upskillz_invoice_mgt_Domain.IRepository;
using Upskillz_invoice_mgt_Domain.IRepository.Base;
using Upskillz_invoice_mgt_Infrastructure.ContextClass;

namespace Upskillz_invoice_mgt_Infrastructure.Repository.Invoice
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly UpskillDbContext _context;
        public InvoiceRepository(UpskillDbContext context)
        {
            _context = context;
        }
        public void Delete(Upskillz_invoice_mgt_Domain.Entities.Invoice entity)
        {
            _context.Invoices.Remove(entity);
        }

        public async Task InsertAsync(Upskillz_invoice_mgt_Domain.Entities.Invoice entity)
        {
           await _context.Invoices.AddAsync(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Upskillz_invoice_mgt_Domain.Entities.Invoice entity)
        {
            _context.Invoices.Update(entity);
        }
    }
}
