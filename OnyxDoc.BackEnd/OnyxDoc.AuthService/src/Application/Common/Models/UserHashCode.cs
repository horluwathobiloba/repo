using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Common.Models
{
    public class UserHashCode
    {
        public string UserEmail { get; set; }
        public int SubscriberId { get; set; }
        public long Time { get; set; }
    }
}
