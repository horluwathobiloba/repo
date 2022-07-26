using System.Collections.Generic;
using Upskillz_invoice_mgt_Domain.Common;

namespace Upskillz_invoice_mgt_Domain.Entities
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string Street { get; set; }
        public long Zip_Code { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}