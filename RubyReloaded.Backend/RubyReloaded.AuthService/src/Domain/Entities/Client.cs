
using RubyReloaded.AuthService.Domain.Common;

namespace RubyReloaded.AuthService.Domain.Entities
{
   public class Client : AuditableEntity
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
