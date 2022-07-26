using System;
using System.Collections.Generic;
using System.Linq;
using Upskillz_invoice_mgt_Domain.Enums;

namespace Upskillz_invoice_mgt_Domain.Entities
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        private decimal Amount;
        public decimal InvoiceAmount 
        {
            get => Amount;
            set => Amount = InvoiceItems.Sum(amount => amount.ItemAmount);
        }
        public AppUser AppUser { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
        public DateTime Issue_Date { get; set; }
        public DateTime Due_Date { get; set; }
    }
}
