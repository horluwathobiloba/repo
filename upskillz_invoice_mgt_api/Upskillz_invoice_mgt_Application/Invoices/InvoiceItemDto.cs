using AutoMapper;
using Upskillz_invoice_mgt_Domain.Entities;

namespace Upskillz_invoice_mgt_Application.Invoices
{
    public class InvoiceItemDto
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvoiceItem, InvoiceItemDto>();
            profile.CreateMap<InvoiceItemDto, InvoiceItem>();
        }
    }
}
