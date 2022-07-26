using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Common.Models
{
    public class UserInviteLinkVm
    {
        public int RoleId { get; set; }
        public int SubscriberId { get; set; }
        public string RecipientEmail { get; set; }
        public string UserId { get; set; }
    }
}
