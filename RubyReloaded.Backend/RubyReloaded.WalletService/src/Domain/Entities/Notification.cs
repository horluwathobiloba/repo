using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Notification: AuditableEntity
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
