using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class TagFAQ:AuditableEntity
    {
        public int FAQQuestionId { get; set; }
        public FAQQuestion FAQQuestion { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
