
using OnyxDoc.AuthService.Domain.Common;

namespace OnyxDoc.AuthService.Domain.Entities
{
   public class Client : AuditableEntity
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
