using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Upskillz_invoice_mgt_Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public Company Company { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
