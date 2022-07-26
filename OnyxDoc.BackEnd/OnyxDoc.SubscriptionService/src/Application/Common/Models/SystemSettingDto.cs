using OnyxDoc.SubscriptionService.Domain.Enums;
using System;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class SystemSettingDto
    {
        public int Id { get; set; }
        public int ExpirationReminder { get; set; }
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
        public string ExpirationSettingsFrequencyDesc { get; set; }
        public bool ShouldSentDocumentsExpire { get; set; }
        public int DocumentExpirationPeriod { get; set; }
        public int WorkflowReminder { get; set; }
        public SettingsFrequency WorkflowReminderSettingsFrequency { get; set; }
        public string WorkflowReminderSettingsFrequencyDesc { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }


        public string Name { get; set; }
        public int SubscriberId { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedById { get; set; }
        public string LastModifiedByEmail { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
    }
}
