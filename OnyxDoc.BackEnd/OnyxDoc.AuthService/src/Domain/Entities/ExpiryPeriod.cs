using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class ExpiryPeriod : AuditableEntity
    {
        public int SystemSettingId { get; set; }
        public int ExpirationReminderInterval { get; set; }
        public SettingsFrequency ExpirationSettingsFrequency { get; set; }
    }
}
