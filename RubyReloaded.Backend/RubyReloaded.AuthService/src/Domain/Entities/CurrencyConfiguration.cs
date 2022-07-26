using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class CurrencyConfiguration: AuditableEntity
    {
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeString { get; set; }
    }
}
