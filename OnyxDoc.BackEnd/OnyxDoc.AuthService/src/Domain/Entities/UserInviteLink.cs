using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class UserInviteLink : AuditableEntity
    {
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public string RecipientEmail { get; set; }
        public string UserEmail { get; set; }
        public string Link { get; set; }

    }
}
