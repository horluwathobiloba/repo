using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class SystemSetting : AuditableEntity
    {
        public ICollection<ExpiryPeriod> ExpirationReminder { get; set; } = new List<ExpiryPeriod>(); 
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
        public string ExpirationSettingsFrequencyDesc { get; set; }
        public bool ShouldSentDocumentsExpire { get; set; }
        public int DocumentExpirationPeriod { get; set; }
        public int WorkflowReminder { get; set; }
        public int SubscriberId { get; set; }
        public SettingsFrequency WorkflowReminderSettingsFrequency { get; set; }
        public string WorkflowReminderSettingsFrequencyDesc { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }

    }
}