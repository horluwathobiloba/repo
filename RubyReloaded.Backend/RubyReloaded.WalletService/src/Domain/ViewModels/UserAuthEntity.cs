using RubyReloaded.WalletService.Domain.Enums;
using System;

namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public abstract class UserAuthEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public string DeviceId { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
    }
}
