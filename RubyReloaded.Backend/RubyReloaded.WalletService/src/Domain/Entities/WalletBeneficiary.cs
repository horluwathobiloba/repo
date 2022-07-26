using RubyReloaded.WalletService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class WalletBeneficiary:AuditableEntity
    {
        public int WalletId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
