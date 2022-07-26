using Upskillz_invoice_mgt_Domain.Common;

namespace Upskillz_invoice_mgt_Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsRead { get; set; }
    }
}