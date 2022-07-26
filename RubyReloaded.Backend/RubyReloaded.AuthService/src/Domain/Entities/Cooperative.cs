using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class Cooperative:AuditableEntity
    {
        public string Email { get; set; }
        public string Description { get; set; }
        public string TermsAndConditions { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string License { get; set; }
        public string CooperativeGoal { get; set; }
        public CooperativeType? CooperativeType { get; set; }
        public int CooperativeSettingId { get; set; }
        public CooperativeSettings CooperativeSetting { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Logo { get; set; }
    }
}
