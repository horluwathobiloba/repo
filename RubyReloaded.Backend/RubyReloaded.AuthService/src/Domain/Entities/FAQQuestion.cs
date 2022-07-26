using RubyReloaded.AuthService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class FAQQuestion : AuditableEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int FAQCategoryId { get; set; }
        public FAQCategory FAQCategory { get; set; }

        public IList<TagFAQ> TagFAQs { get; set; }
    }
}
