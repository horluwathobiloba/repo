using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Documents.Queries.GetDocuments
{
    public class ExpiryPeriodDto
    {
        public int Id { get; set; }
        public int SystemSettingId { get; set; }
        public int ExpirationReminderInterval { get; set; }
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
    }
}
