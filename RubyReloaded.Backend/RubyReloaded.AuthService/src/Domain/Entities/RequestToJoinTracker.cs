using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    //This is the table for private cooperatives which users have requested to join
    public class RequestToJoinTracker:AuditableEntity
    {
        public string UserEmail { get; set; }
        public CooperativeAccessStatus CooperativeAccessStatus { get; set; }
        public string AdminEmail { get; set; }
        public int CooperativeId{ get; set; }
        //public RequestType RequestType{ get; set; }
    }
}
