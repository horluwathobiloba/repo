using AutoMapper;
using OnyxDoc.DocumentService.Application.Common.Mappings;
using OnyxDoc.DocumentService.Domain.Common;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments
{
    public class SystemSettingDto 
    {
        public int Id { get; set; }
        public List<ExpiryPeriodDto> ExpirationReminder { get; set; }
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
