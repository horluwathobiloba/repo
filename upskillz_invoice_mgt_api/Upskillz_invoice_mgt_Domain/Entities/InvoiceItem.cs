using Upskillz_invoice_mgt_Domain.Common;

namespace Upskillz_invoice_mgt_Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int Itemunit { get; set; }
        public decimal ItemPrice { get; set; }
        private decimal Amount;
        public decimal ItemAmount
        {
            get => Amount;
            set => Amount = Itemunit * ItemPrice;
        }

    }
}