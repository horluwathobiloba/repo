
using Onyx.AuthService.Domain.Common;

namespace Onyx.AuthService.Domain.Entities
{
   public class Client : AuditableEntity
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
