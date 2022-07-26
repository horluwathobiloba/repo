using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Domain.Entities
{
    public class UserApproverRequest:AuditableEntity
    {
        public string HashLink { get; set; }
        public string UserEmail { get; set; }
        public string ApproverEmail { get; set; }
        public UserCreationStatus UserCreationStatus { get; set; }
        public string UserCreationStatusDesc { get; set; }
    }
}
